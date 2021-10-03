using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenBehaviour : MonoBehaviour
{
    public GameObject labelPrefab, stocksEmpty, price, range,portfolio;
    GameManager gm;
    GlobalControlScript gcs;
    public int selection = 0;
    public Stock[] stocks;
    // Start is called before the first frame update
    void Start()
    {

        print("Started");
        gcs = GameObject.Find("GlobalControl").GetComponent<GlobalControlScript>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        foreach(Stock s in stocks){
            GameObject label = Instantiate(labelPrefab,stocksEmpty.transform);
            TextMeshProUGUI tmp = label.GetComponent<TextMeshProUGUI>();
            tmp.text = s.symbol;
            label.transform.position += (Vector3.down) * (4*selection++);
        }
        for(int i=0; i<15; i++){
            gm.AdvanceTurn(stocks);
        }
        selection=-1;
        ChangeSelection(false);
        ChangeSelection(true);

    }

    void Update()
    {
       if(gcs.GetKeyDown("up"))ChangeSelection(true);
       else if(gcs.GetKeyDown("down"))ChangeSelection(false);
       UpdatePos();
    }

    void ChangeSelection(bool up){
        
        TextMeshProUGUI[] tmp;
        tmp = stocksEmpty.GetComponentsInChildren<TextMeshProUGUI>();
        
        if(selection != -1)
        {
            tmp[selection].gameObject.transform.position += Vector3.right*4;
            tmp[selection].text = stocks[selection].symbol;
        }
        else{
            stocksEmpty.transform.position += Vector3.up*(4*(up?1:-1));
        }
        selection+=up?-1:1;
        stocksEmpty.transform.position += Vector3.up*(4*(up?-1:1));

        if(selection<0){
            selection=0;
            stocksEmpty.transform.position += Vector3.up*(4*(up?1:-1));
        }
        else if(selection>=stocks.Length){
            selection=stocks.Length-1;
            stocksEmpty.transform.position += Vector3.up*(4*(up?1:-1));
        }
        tmp[selection].gameObject.transform.position += Vector3.left*4;

        tmp[selection].text = stocks[selection].name + "("+stocks[selection].symbol+")";

        UpdateGraph();
    }

    public void UpdateGraph(){
        RectTransform ra=null, rb=null;
        float max = stocks[selection].history.Max();
        float min = stocks[selection].history.Min();
        for(int i=0; i<5; i++){
            float histPercent = (stocks[selection].history[i] - min)/(max-min); 

            GameObject go = GameObject.Find("point ("+i+")");
            if(ra!=null) rb = ra;
            ra = go.GetComponent<RectTransform>();
            ra.anchoredPosition = new Vector3(20*(i+1),(histPercent*100)-50,0);

            if(i!=0){
                GameObject line = GameObject.Find("line ("+(i-1)+")");
                RectTransform ret = line.GetComponent<RectTransform>();
                Vector3 v3 = rb.anchoredPosition-ra.anchoredPosition;
                line.transform.rotation = Quaternion.identity;
                line.transform.Rotate(0,0,Vector3.SignedAngle(v3,line.transform.up,Vector3.forward)+90);

                line.transform.localScale = new Vector3 (v3.magnitude,2,2);
                ret.anchoredPosition = new Vector3(20*(i+1)-10,(ra.anchoredPosition.y+rb.anchoredPosition.y)/2,1);
                
            } 
        }
        min = Mathf.Round(min*100)/100;
        max = Mathf.Round(max*100)/100;
        price.GetComponent<TextMeshProUGUI>().text = "$"+Mathf.Round(stocks[selection].price*100)/100;
        range.GetComponent<TextMeshProUGUI>().text = min+" ~ "+max;
     
    }

    public void UpdatePos(){
        portfolio.GetComponent<TextMeshProUGUI>().text="";
        foreach(Stock s in stocks){
            if(s.owned>0) portfolio.GetComponent<TextMeshProUGUI>().text+=s.symbol+": "+s.owned+" @ "+s.price+"\n";
        }
    }

    public void Buy(){
        if(gm.cap < stocks[selection].price * gm.buyAmount)gm.Toast("Insufficient Funds");
        else{
            gm.cap -= stocks[selection].price * gm.buyAmount;
            stocks[selection].owned += gm.buyAmount;
            gm.AdvanceTurn(stocks);
        }
    }
    public void Sell(){
        if(stocks[selection].owned<gm.buyAmount)gm.Toast("You haven't got it to sell");
        else{
            gm.cap += stocks[selection].price * gm.buyAmount;
            stocks[selection].owned -= gm.buyAmount;
            gm.AdvanceTurn(stocks);
        }
    }
}
