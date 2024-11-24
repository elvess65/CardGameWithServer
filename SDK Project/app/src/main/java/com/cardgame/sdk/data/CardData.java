package com.cardgame.sdk.data;

public class CardData {
    public int DataBaseId;
    public int ID;
    public int BodyId;
    public int FaceId;
    public int HairId;
    public int KitId;
    public int Power;
    public int Type;
    public int Popularity;
    public String CardName;

    public CardData(int dataBaseID, int bodyId, int faceId, int hairId, int kitId, int power, int type, String cardName) {
        DataBaseId = dataBaseID;
        BodyId = bodyId;
        FaceId = faceId;
        HairId = hairId;
        KitId = kitId;
        Power = power;
        Type = type;
        CardName = cardName;
        Popularity = 0;
        ID = 0;
    }

    public CardData(CardData cardData, int instanceID)
    {
        if (cardData == null)
            return;

        ID = instanceID;

        DataBaseId = cardData.DataBaseId;
        BodyId = cardData.BodyId;
        FaceId = cardData.FaceId;
        HairId = cardData.HairId;
        KitId = cardData.KitId;
        Power = cardData.Power;
        Type = cardData.Type;
        Popularity = cardData.Popularity;
        CardName = cardData.CardName;
    }

    public int getPopularity() {
        return Popularity;
    }
}
