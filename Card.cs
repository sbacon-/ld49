using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card  
{
    public CardType type;
    public Effect effect;
    public string name;
    public int level;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate(){
        
    }
    public void Combine(){

    }
    public void Discard(){

    }
    public bool Compare(Card c){
        return level==c.level && name==c.name;
    }
    public void Draw(){
        type = (CardType) Random.Range(0,7);
        name = type.ToString();
        level = 1;
        switch(type){
            case CardType.TheMoon:
                effect = Effect.MOON;
                break;
            case CardType.Bogdanoff:
                effect = Effect.BOG;
                break;
            case CardType.Bull:
                effect = Effect.PUMP;
                break;
            case CardType.Bear:
                effect = Effect.DUMP;
                break;
            case CardType.Cycle:
                effect = Effect.WHEEL;
                break;
            case CardType.Stability:
                effect = Effect.STAB;
                break;
            case CardType.TaxEvasion:
                effect = Effect.TAX;
                break;
        }
    }
}
public enum CardType{
    TheMoon, Bogdanoff, Bull, Bear, Cycle, Stability, TaxEvasion
}
public enum Effect{
    PUMP, DUMP, BOG, WHEEL, TAX, STAB, MOON
}


