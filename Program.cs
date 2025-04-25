using System;
using System.Speech.Recognition;
using System.Threading;

namespace DS
{
    internal class Program
    {
        private static SpeechRecognitionEngine recognizer;

        static void Main(string[] args)
        {
            // Initialize the Speech Recognizer
            InitializeSpeechRecognition();

            // Keep the console open and listen for commands
            Console.WriteLine("Virtual Assistant: Listening for commands...");
            Console.WriteLine("Say 'hello' to get started!");

            // Prevent the application from closing immediately
            Thread.Sleep(Timeout.Infinite);
        }

        private static void InitializeSpeechRecognition()
        {
            // Create a new SpeechRecognitionEngine object
            recognizer = new SpeechRecognitionEngine();

            // Set the input to the default microphone
            recognizer.SetInputToDefaultAudioDevice();

            // Define a set of commands the recognizer will recognize
            var commands = new Choices("hello", "open notepad", "exit");

            // Create a Grammar based on the defined commands
            var grammar = new Grammar(new GrammarBuilder(commands));

            // Load the grammar into the recognizer
            recognizer.LoadGrammar(grammar);

            // Event handler for when speech is recognized
            recognizer.SpeechRecognized += Recognizer_SpeechRecognized;

            // Start asynchronous recognition
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        private static void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string recognizedText = e.Result.Text.ToLower(); // Convert speech to lowercase for easier matching

            // Handle recognized commands
            if (recognizedText.Contains("hello"))
            {
                Console.WriteLine("Virtual Assistant: Hello! How can I assist you?");
            }
            else if (recognizedText.Contains("open notepad"))
            {
                OpenApplication("notepad");
            }
            else if (recognizedText.Contains("exit"))
            {
                Console.WriteLine("Virtual Assistant: Goodbye!");
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine($"Virtual Assistant: I didn't understand '{recognizedText}'. Try again!");
            }
        }

        // Method to launch an application
        private static void OpenApplication(string appName)
        {
            try
            {
                // Try to launch the application
                System.Diagnostics.Process.Start(appName);
                Console.WriteLine($"Virtual Assistant: Opening {appName}...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Virtual Assistant: Sorry, I couldn't open {appName}. Error: {ex.Message}");
            }
        }
    }
}
