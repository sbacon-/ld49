using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

public class GameManager : MonoBehaviour
{
    GlobalControlScript gcs;
    public ScreenBehaviour sb;
    public string sceneMusic;

    float net = 100000f;
    public float cap = 100000f;

    int time = -14;
    int selectedCard = -1;

    public int buyAmount = 1;



    

    // Start is called before the first frame update
    void Start()
    {
        gcs = GameObject.Find("GlobalControl").GetComponent<GlobalControlScript>();
        gcs.PlaySound(sceneMusic); 

    }

    // Update is called once per frame
    void Update()
    {
        net = cap;
        foreach(Stock s in sb.stocks){
            net+=s.owned*s.price;
        }


        GameObject netText = GameObject.Find("net");
        GameObject capText = GameObject.Find("capital");
        GameObject timeText = GameObject.Find("time");
        GameObject amtText = GameObject.Find("buyAmt");
        if(amtText != null) amtText.GetComponent<TextMeshProUGUI>().text=buyAmount+"";
        if(timeText != null) timeText.GetComponent<TextMeshProUGUI>().text=time+"";
        if(capText != null) capText.GetComponent<TextMeshProUGUI>().text="$"+Mathf.Round(net*100)/100; 
        if(netText != null) netText.GetComponent<TextMeshProUGUI>().text="$"+Mathf.Round(cap*100)/100; 

       if(gcs.GetKeyDown("left"))ChangeAmt(true);
       else if(gcs.GetKeyDown("right"))ChangeAmt(false);
    }

    public void AdvanceTurn(Stock[] stocks){
       foreach(Stock s in stocks){
           switch (s.trend){
            case Trend.PUSH:
                s.price*=Random.Range(0.99f,1.01f);
                break;
            case Trend.RISE:
                s.price*=Random.Range(1.01f,1.05f);
                break;
            case Trend.FALL:
                s.price*=Random.Range(0.95f,0.99f);
                break;
            case Trend.MOON:
                s.price*=Random.Range(1.10f,1.30f);
                break;
            case Trend.CRASH:
                s.price*=Random.Range(0.90f,0.70f);
                break;
           } 
           for(int i = 0; i<4; i++) s.history[i] = s.history[i+1];
           s.history[4] = s.price;
       } 
       time++;
       sb.UpdateGraph();
       foreach(Stock s in sb.stocks){
           s.trend = Trend.PUSH;
       }
    }
    public void AdvanceTurn(){
        AdvanceTurn(sb.stocks);
    }

    public void SelectCard(int index){
        GameObject card = GameObject.Find(""+index);
        if(selectedCard != -1){
            GameObject oldCard = GameObject.Find(""+selectedCard);
            oldCard.GetComponent<RectTransform>().anchoredPosition = 
             new Vector2((selectedCard*175)-350,0);
        }
        if(index==-1){
            card.GetComponent<RectTransform>().anchoredPosition = 
                new Vector2(0,300);
        }else{
            card.GetComponent<RectTransform>().anchoredPosition = 
                new Vector2((index*175)-350,200);
        }
        selectedCard = index;
    }

    public void Buy(){
        sb.Buy();
    }
    public void Sell(){
        sb.Sell();
    }
    public void Toast(string s){
       GameObject.Find("toast").GetComponent<ToastBehaviour>().Toast(s); 
    }

    void ChangeAmt(bool left){
        if(left)buyAmount/=10; 
        if(!left)buyAmount*=10; 
        if(buyAmount<1)buyAmount=1;
        if(buyAmount>1000000)buyAmount=1000000;
    }

    public void Activate(Effect effect, int level){
        bool fail = Random.Range(0,level+1)==0;
        if(fail){
            Toast("Failure");
        }else{
            Toast("Success");
        };
        switch(effect){
            case Effect.PUMP:
                sb.stocks[sb.selection].trend = fail?Trend.PUSH : Trend.RISE;
                break;
            case Effect.DUMP:
                sb.stocks[sb.selection].trend = fail?Trend.PUSH : Trend.FALL;
                break;
            case Effect.BOG:
                bool domp = Random.Range(0,2)==0;
                if(!fail){
                    if(domp){
                        Toast("DOMP EET");
                        sb.stocks[sb.selection].trend = Trend.CRASH;
                    }else{
                        Toast("POMP EET");
                        sb.stocks[sb.selection].trend = Trend.MOON;
                    }
                }
                break;
            case Effect.WHEEL:
                if(!fail){
                    switch(Random.Range(0,4)){
                        case 0:
                            Toast("Recession");
                            sb.stocks[sb.selection].trend = Trend.FALL;
                            break;
                        case 1:
                            Toast("Recovery");
                            sb.stocks[sb.selection].trend = Trend.RISE;
                            break;
                        case 2:
                            Toast("Boom");
                            sb.stocks[sb.selection].trend = Trend.MOON;
                            break;
                        case 3:
                            Toast("Bust");
                            sb.stocks[sb.selection].trend = Trend.CRASH;
                            break;
                    }
                }
                break;
            case Effect.TAX:
                if(fail){
                    cap-=3000;
                }else{
                    cap+=30000*level;
                }
                break;
            case Effect.MOON:
                sb.stocks[sb.selection].trend = Trend.MOON;
                break;
        }
        AdvanceTurn();
    }

    public bool Combine(){
        CardBehaviour cb = GameObject.Find(selectedCard+"").GetComponent<CardBehaviour>();
        Card c = cb.card;
        for(int i=0; i<5; i++){
            if(i==selectedCard)continue;
            CardBehaviour cb2 = GameObject.Find(i+"").GetComponent<CardBehaviour>();
            if(c.Compare(cb2.card)){
                c.level++;
                cb2.Draw();
                AdvanceTurn();
                return true;
            }
        }
        return false;
    }
}
