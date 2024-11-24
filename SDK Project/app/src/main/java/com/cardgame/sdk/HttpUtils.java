package com.cardgame.sdk;

import com.google.gson.Gson;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;

public class HttpUtils {

    public static String sendGetRequest(String urlString) throws Exception {
        // Create URL object
        URL url = new URL(urlString);

        // Open connection
        HttpURLConnection connection = (HttpURLConnection) url.openConnection();

        // Set request method to GET
        connection.setRequestMethod("GET");


        // Get response code
        int responseCode = connection.getResponseCode();
        if (responseCode != HttpURLConnection.HTTP_OK) { // 200 OK
            throw new Exception("GET request failed with response code: " + responseCode);
        }

        // Read the response
        BufferedReader reader = new BufferedReader(new InputStreamReader(connection.getInputStream()));
        StringBuilder response = new StringBuilder();
        String line;
        while ((line = reader.readLine()) != null) {
            response.append(line);
        }

        // Close reader and connection
        reader.close();
        connection.disconnect();

        return response.toString();
    }

    public static String sendPostRequest(String urlString, int value) throws Exception {
        // Create URL object
        URL url = new URL(urlString);

        // Open connection
        HttpURLConnection connection = (HttpURLConnection) url.openConnection();

        // Set request method to POST
        connection.setRequestMethod("POST");

        // Set headers
        connection.setRequestProperty("Content-Type", "text/plain; charset=UTF-8");
        connection.setRequestProperty("Accept", "text/plain");

        // Enable sending data
        connection.setDoOutput(true);

        // Write the integer as the request body
        try (OutputStream os = connection.getOutputStream()) {
            String data = String.valueOf(value); // Convert int to String
            os.write(data.getBytes("UTF-8"));
            os.flush();
        }

        // Get response code
        int responseCode = connection.getResponseCode();
        if (responseCode != HttpURLConnection.HTTP_OK) { // 200 OK
            throw new Exception("POST request failed with response code: " + responseCode);
        }

        // Read the response
        StringBuilder response = new StringBuilder();
        try (var reader = new java.io.BufferedReader(new java.io.InputStreamReader(connection.getInputStream()))) {
            String line;
            while ((line = reader.readLine()) != null) {
                response.append(line);
            }
        }

        // Disconnect
        connection.disconnect();

        return response.toString();
    }
}