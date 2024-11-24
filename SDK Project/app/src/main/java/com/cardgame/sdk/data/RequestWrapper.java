package com.cardgame.sdk.data;

public class RequestWrapper {
    public PlayerData Player;
    public PlayerData Enemy;

    public RequestWrapper(PlayerData player, PlayerData enemy) {
        this.Player = player;
        this.Enemy = enemy;
    }

}
