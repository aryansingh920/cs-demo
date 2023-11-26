using System;
using System.Net;
using System.Threading;

class SimpleHttpServer
{
    static void Main()
    {
        // Set up the listener on http://localhost:8080/
        string url = "http://localhost:8080/";
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add(url);

        Console.WriteLine($"Listening for requests on {url}");

        listener.Start();

        while (true)
        {
            // Wait for a request to come in
            HttpListenerContext context = listener.GetContext();

            // Process the request on a separate thread to handle multiple requests concurrently
            ThreadPool.QueueUserWorkItem((o) => HandleRequest(context));
        }
    }

    static void HandleRequest(HttpListenerContext context)
    {
        // Get the request and response objects
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;

        // Set up the response
        response.ContentType = "text/plain";
        response.StatusCode = 200;

        // Get the client's IP address
        string clientIP = request.RemoteEndPoint.Address.ToString();

        // Construct the response message
        string responseMessage = $"Hello, {clientIP}! This is the C# HTTP server.";

        // Convert the response message to bytes
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseMessage);

        // Get the response stream and write the response
        response.OutputStream.Write(buffer, 0, buffer.Length);
        response.Close();
    }
}
