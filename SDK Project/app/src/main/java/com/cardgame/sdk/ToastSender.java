package com.cardgame.sdk;

import android.widget.Toast;

public class ToastSender {
    public void ShowToast(String text, boolean isLongMessage) {
        Toast.makeText(UnityBridge.unityActivity, text, isLongMessage ? Toast.LENGTH_LONG : Toast.LENGTH_SHORT).show();
    }
}