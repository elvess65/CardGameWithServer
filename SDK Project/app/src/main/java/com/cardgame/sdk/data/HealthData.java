package com.cardgame.sdk.data;

public class HealthData {
    public int CurrentHealth;
    public int MaxHealth;

    public HealthData(int maxHealth) {
        this.CurrentHealth = maxHealth;
        this.MaxHealth = maxHealth;
    }
}
