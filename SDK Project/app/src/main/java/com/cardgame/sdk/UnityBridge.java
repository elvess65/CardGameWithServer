package com.cardgame.sdk;

import android.app.Activity;

public class UnityBridge {
    public static Activity unityActivity;

    public static void GetActivity(Activity activity) {
        unityActivity = activity;
    }
}
