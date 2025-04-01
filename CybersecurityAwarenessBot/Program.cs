using System;
using System.Media;
using System.Threading;
using System.Collections.Generic;
using System.Speech.Synthesis;

namespace CybersecurityAwarenessBot
{
    class Program
    {

        // Automatic properties for user information
        public static string UserName { get; set; }
        public static bool IsRunning { get; set; } = true;

        // Dictionary to store predefined topic content with definitions, examples, and activities
        private static Dictionary<string, (string definition, string example, string activity, string icon)> topicContent =
    new Dictionary<string, (string, string, string, string)>(StringComparer.OrdinalIgnoreCase)
{
    { "password", (
        "Strong passwords are crucial! They should be at least 12 characters long, include uppercase and lowercase letters, numbers, and special characters. Never reuse passwords across different accounts. Consider using a password manager!",
        "Example: 'Password123' is weak, but 'T4k3!C@r3_0f-Y0ur$3lf' is strong because it mixes character types and is long enough to resist brute force attacks.",
        "Activity: Create a strong password for a fictional account following the guidelines. Then, check its strength using the NIST guidelines: 1) At least 12 characters 2) Mix of character types 3) No common dictionary words 4) Not based on personal information.",
        @"
    +=========+
    |  *****  |
    |  * * *  |
    |  *****  |
    +==+   +==+"
    )},
    { "phishing", (
        "Phishing attacks try to trick you into revealing sensitive information. Always verify the sender's email address, be suspicious of unexpected attachments, and never click on suspicious links. Legitimate organizations won't ask for your password via email.",
        "Example: You receive an email claiming to be from your bank stating 'Your account has been compromised, click here to verify your information.' The link leads to a fake website designed to steal your credentials.",
        "Activity: Review these email subjects and identify which ones might be phishing attempts:\n1. 'Your Amazon order #12345 has shipped'\n2. 'URGENT: Your account access will be terminated'\n3. 'Netflix: Update your payment information'\n4. 'Meeting notes from yesterday'",
        @"
    .---------.
    |><)))'>   |
    '---------'"
    )},
    { "safe browsing", (
        "For safe browsing: keep your browser updated, use HTTPS websites, be careful what you download, avoid public Wi-Fi for sensitive transactions, and consider using a VPN for added security.",
        "Example: When shopping online, always check that the website URL begins with 'https://' and has a padlock icon in the address bar before entering payment information.",
        "Activity: Visit a favorite website and check: 1) Is it using HTTPS? 2) Can you find their privacy policy? 3) Does the website ask for unnecessary personal information? 4) Are there any suspicious pop-ups or redirects?",
        @"
    .-----------. 
    |  .-----.  |
    |  | [P] |  |
    |  '-----'  |
    '-----------'"
    )},
    { "malware", (
        "Malware is malicious software designed to harm your system. Protect yourself by keeping your OS and software updated, using antivirus, avoiding suspicious downloads, and backing up your data regularly.",
        "Example: A free game download from an unofficial site might contain a trojan that secretly installs a keylogger to capture your passwords and financial information.",
        "Activity: Create a personal malware prevention checklist including: 1) Software that needs regular updates 2) Backup schedule for important files 3) Safe download sources for programs you commonly use 4) Warning signs of potential infection",
        @"
    +----------+
    |  /\__/\  |
    |  (XX)  |
    |  //\\  |
    +----------+"
    )},
    { "2fa", (
        "Two-Factor Authentication (2FA) adds an extra layer of security by requiring something you know (password) and something you have (like your phone). Enable it whenever possible on your accounts!",
        "Example: After entering your password on a website, you receive a text message with a 6-digit code that you must also enter to complete the login process.",
        "Activity: Make a list of your 5 most important online accounts and check which ones support 2FA. For those that do, plan to enable it within the next week. For those that don't, consider if there are alternative services that offer better security.",
        @"
    .-----. .-----.
    | ### | | 123 |
    | ### | | 456 |
    | ### | | 789 |
    '-----' '-----'"
    )},
    { "vpn", (
        "A Virtual Private Network (VPN) encrypts your internet connection, protecting your data from prying eyes. It's essential when using public Wi-Fi and helps maintain privacy online.",
        "Example: When using coffee shop Wi-Fi, a VPN creates an encrypted tunnel for your data, preventing others on the same network from seeing your online activities or intercepting your information.",
        "Activity: Research 3 reputable VPN services and compare their features: 1) Privacy policy 2) Connection speed 3) Server locations 4) Price 5) Device compatibility",
        @"
    .-----------. 
    | # -> # -> |
    | v # v #   |
    | [=======] |
    '-----------'"
    )},
    { "ransomware", (
        "Ransomware encrypts your files and demands payment for the decryption key. Prevent it by keeping software updated, backing up data regularly, and avoiding suspicious downloads.",
        "Example: After opening an attachment from an unknown email, your computer screen displays a message demanding $500 in Bitcoin to unlock your now-encrypted files.",
        "Activity: Create a ransomware response plan: 1) Which files would you prioritize backing up? 2) How often should they be backed up? 3) Where would you store backups (cloud/physical)? 4) What steps would you take if infected?",
        @"
    +----------+
    | .-. .-.  |
    | |$| |$|  |
    | '-' '-'  |
    +----------+"
    )}
};

        // Dictionary to store basic responses
        private static Dictionary<string, string> responses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "hello", "Hello there, {0}! What cybersecurity topic would you like to discuss?" },
            { "hi", "Hi {0}! What cybersecurity topic would you like to discuss?" },
            { "how are you", "I'm functioning perfectly, {0}! What cybersecurity topic would you like to discuss?" },
            { "what's your purpose", "I'm designed to help raise awareness about cybersecurity issues and provide helpful tips to keep you safe online. What cybersecurity topic would you like to discuss?" },
            { "what can i ask you about", "You can ask me about password safety, phishing attacks, safe browsing habits, and other cybersecurity topics. What would you like to discuss?" },
            { "help", "I can provide information on various cybersecurity topics. You can ask about 'password safety', 'phishing', 'safe browsing', '2FA', 'malware', 'vpn', or 'ransomware'. What topic interests you?" },
            { "bye", "Goodbye, {0}! Stay safe online!" },
            { "exit", "Exiting the Cybersecurity Awareness Bot. Remember to stay vigilant online, {0}!" }
        };

        static void Main(string[] args)
        {
            // Set console properties
            Console.Title = "◢◤ CYBERSECURITY AWARENESS BOT ◥◣";
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            // Display the animated background - ADD THIS LINE
            DrawNeonBackground();

            // Play voice greeting
            PlayVoiceGreeting();

            // Display Future Tech style ASCII logo
            DisplayFutureTechLogo();

            // Display loading animation
            DisplayLoadingAnimation();

            // Display welcome message with typing effect
            TypeWriteEffect("\n⚡ Welcome to the Cybersecurity Awareness Chatbot, known as CYBERTITAN ⚡", ConsoleColor.Cyan);
            Console.WriteLine();

            // Ask for user's name
            GetUserName();

            // Ask what topic they want to discuss after getting their name
            AskForTopic();

            // Main chat loop
            ChatLoop();
        }

        static void DrawNeonBackground()
        {
            // Save the current console properties
            int origWidth = Console.WindowWidth;
            int origHeight = Console.WindowHeight;
            ConsoleColor defaultForeground = Console.ForegroundColor;
            ConsoleColor defaultBackground = Console.BackgroundColor;

            // Set console properties for the animation
            Console.CursorVisible = false;

            // Characters for the matrix-like digital rain effect
            char[] neonChars = { '0', '1', '@', '#', '$', '%', '&', '*', '!', '+', '=', '?', ':', ';' };

            // Colors for the neon effect
            ConsoleColor[] neonColors = {
        ConsoleColor.Cyan,
        ConsoleColor.Magenta,
        ConsoleColor.DarkCyan,
        ConsoleColor.DarkMagenta,
        ConsoleColor.Blue,
        ConsoleColor.DarkBlue
    };

            // Create arrays to track digital rain properties
            int[] startPositions = new int[origWidth];
            int[] lengths = new int[origWidth];
            int[] speeds = new int[origWidth];
            int[] colorIndices = new int[origWidth];

            // Initialize the digital rain properties
            Random random = new Random();
            for (int i = 0; i < startPositions.Length; i++)
            {
                startPositions[i] = random.Next(-origHeight, 0);
                lengths[i] = random.Next(3, 10);
                speeds[i] = random.Next(1, 3);
                colorIndices[i] = random.Next(0, neonColors.Length);
            }

            // Draw the background for 3 seconds
            DateTime endTime = DateTime.Now.AddSeconds(3);
            while (DateTime.Now < endTime)
            {
                // Clear for redrawing
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();

                // Draw neon grid lines
                if (random.Next(0, 10) < 3) // Occasionally show grid
                {
                    // Draw horizontal grid lines
                    for (int y = 0; y < origHeight; y += 5)
                    {
                        Console.ForegroundColor = neonColors[random.Next(0, neonColors.Length)];
                        for (int x = 0; x < origWidth; x++)
                        {
                            if (x % 3 == 0) // Make grid dotted
                            {
                                Console.SetCursorPosition(x, y);
                                Console.Write('·');
                            }
                        }
                    }

                    // Draw vertical grid lines
                    for (int x = 0; x < origWidth; x += 10)
                    {
                        Console.ForegroundColor = neonColors[random.Next(0, neonColors.Length)];
                        for (int y = 0; y < origHeight; y++)
                        {
                            if (y % 2 == 0) // Make grid dotted
                            {
                                Console.SetCursorPosition(x, y);
                                Console.Write('|');
                            }
                        }
                    }
                }

                // Draw digital rain
                for (int i = 0; i < startPositions.Length; i++)
                {
                    // Only draw some columns for sparse effect
                    if (i % 3 != 0) continue;

                    // Update the rain position
                    startPositions[i] += speeds[i];

                    // Reset if the rain has moved off screen
                    if (startPositions[i] - lengths[i] > origHeight)
                    {
                        startPositions[i] = random.Next(-origHeight, 0);
                        lengths[i] = random.Next(3, 10);
                        speeds[i] = random.Next(1, 3);
                        colorIndices[i] = random.Next(0, neonColors.Length);
                    }

                    // Draw the digital rain characters
                    Console.ForegroundColor = neonColors[colorIndices[i]];
                    for (int j = 0; j < lengths[i]; j++)
                    {
                        int y = startPositions[i] - j;
                        if (y >= 0 && y < origHeight)
                        {
                            Console.SetCursorPosition(i, y);
                            Console.Write(neonChars[random.Next(0, neonChars.Length)]);
                        }
                    }
                }

                // Draw cybersecurity symbols randomly
                if (random.Next(0, 10) < 2) // Occasionally show symbols
                {
                    int symbolX = random.Next(0, origWidth - 10);
                    int symbolY = random.Next(0, origHeight - 5);
                    ConsoleColor symbolColor = neonColors[random.Next(0, neonColors.Length)];

                    string[] symbols = {
                @"  .---.   
 /   /    
[___]     ",
                @"  _____  
 |     | 
 |_____| ",
                @"  .===.  
 ///// 
 \\\\\  ",
                @"  _|_|_  
 |_____| 
 |_____| "
            };

                    string symbol = symbols[random.Next(0, symbols.Length)];
                    string[] lines = symbol.Split('\n');

                    Console.ForegroundColor = symbolColor;
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (symbolY + i < origHeight)
                        {
                            Console.SetCursorPosition(symbolX, symbolY + i);
                            Console.Write(lines[i]);
                        }
                    }
                }

                // Add glowing edges
                Console.ForegroundColor = neonColors[random.Next(0, neonColors.Length)];
                for (int i = 0; i < origWidth; i++)
                {
                    Console.SetCursorPosition(i, 0);
                    Console.Write('▄');
                    Console.SetCursorPosition(i, origHeight - 1);
                    Console.Write('▀');
                }

                for (int i = 1; i < origHeight - 1; i++)
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write('█');
                    Console.SetCursorPosition(origWidth - 1, i);
                    Console.Write('█');
                }

                Thread.Sleep(100);
            }

            // Restore console properties
            Console.CursorVisible = true;
            Console.ForegroundColor = defaultForeground;
            Console.BackgroundColor = defaultBackground;
            Console.Clear();
        }

        static void PlayVoiceGreeting()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Initializing audio systems...");
                Console.ResetColor();

                // Create a SpeechSynthesizer object
                SpeechSynthesizer synth = new SpeechSynthesizer();

                // Configure the synthesizer
                synth.Volume = 100; // Set volume (0-100)
                synth.Rate = 0;     // Set speaking rate (-10 to 10)

                // Speak a greeting message
                synth.Speak($"Welcome to the Cybersecurity Awareness Bot.  My name is Cyber Titan, your guardian of the digital realm. Please identify yourself to continue.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error playing audio: {ex.Message}");
                Console.ResetColor();
                Thread.Sleep(1000);
            }
        }

        static void DisplayFutureTechLogo()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
╔══════════════════════════════════════════════════════════════════════════════════════╗
║                                                                                      ║");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(@"
║   ██████╗██╗   ██╗██████╗ ███████╗██████╗ ████████╗██╗████████╗ █████╗ ███╗   ██╗  ║
║  ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗╚══██╔══╝██║╚══██╔══╝██╔══██╗████╗  ██║  ║
║  ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝   ██║   ██║   ██║   ███████║██╔██╗ ██║  ║
║  ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗   ██║   ██║   ██║   ██╔══██║██║╚██╗██║  ║
║  ╚██████╗   ██║   ██████╔╝███████╗██║  ██║   ██║   ██║   ██║   ██║  ██║██║ ╚████║  ║
║   ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝   ╚═╝   ╚═╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═══╝  ║");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(@"
║                                                                                      ║
║               🔒 VIGILANCE NEVER SLEEPS 🔒                                         ║
║                                                                                      ║
║             [CYBER SECURITY INTELLIGENCE v1.0]                                       ║
╚══════════════════════════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }
        static void DisplayLoadingAnimation()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("\nInitializing system");

            // Loading strip animation
            string[] frames = { "⠋", "⠙", "⠹", "⠸", "⠼", "⠴", "⠦", "⠧", "⠇", "⠏" };
            for (int i = 0; i < 20; i++)
            {
                Console.Write($" {frames[i % frames.Length]}");
                Thread.Sleep(100);
                Console.Write("\b\b");
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" [SYSTEM ONLINE]");
            Console.ResetColor();

            // Progress bar with neon color effect
            Console.Write("\n[");
            for (int i = 0; i < 30; i++)
            {
                // Alternating between cyan and magenta for neon effect
                Console.ForegroundColor = i % 2 == 0 ? ConsoleColor.Cyan : ConsoleColor.Magenta;
                Console.Write("█");
                Thread.Sleep(30);
            }
            Console.ResetColor();
            Console.WriteLine("] 100%");

            Thread.Sleep(500);
        }

        static void GetUserName()
        {
            bool validName = false;

            while (!validName)
            {
                // Decorative border for user prompt
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n╔════════════════════════════════════════════════════════╗");
                Console.WriteLine("║            USER IDENTIFICATION REQUIRED                ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════╝");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\n→ Enter your name: ");
                Console.ResetColor();

                UserName = Console.ReadLine().Trim();

                if (string.IsNullOrWhiteSpace(UserName))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[ERROR] Identity verification failed. Retry required.");
                    Console.ResetColor();
                }
                else
                {
                    validName = true;
                    Console.ForegroundColor = ConsoleColor.Green;
                    TypeWriteEffect($"\n>>> Identity confirmed: {UserName} <<<", ConsoleColor.Green);
                    TypeWriteEffect(">>> Cybersecurity protocols activated <<<", ConsoleColor.Green);
                }
            }
        }

        static void AskForTopic()
        {
            // Decorative border for topic selection
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("║              CHOOSE A CYBERSECURITY TOPIC               ║");
            Console.WriteLine("║                                                         ║");
            Console.WriteLine("║  • password   • phishing   • safe browsing   • 2fa      ║");
            Console.WriteLine("║  • malware    • vpn        • ransomware                 ║");
            Console.WriteLine("║                                                         ║");
            Console.WriteLine("║  Type 'help' for command list                           ║");
            Console.WriteLine("║  Type 'exit' to terminate session                       ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            TypeWriteEffect($"\n[CYBER-SEC v2.077] → Hello {UserName}! What cybersecurity topic would you like to discuss?", ConsoleColor.Green);
            Console.ResetColor();
        }

        // THIS IS THE MISSING METHOD THAT WAS CAUSING THE ISSUE
        static void TypeWriteEffect(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(10); // Adjust speed as needed
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        static void ProcessInput(string input)
        {
            // Check if the input is an exit command
            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("bye", StringComparison.OrdinalIgnoreCase))
            {
                string response = responses.ContainsKey(input) ?
                    string.Format(responses[input], UserName) :
                    $"Goodbye, {UserName}! Stay safe online!";

                TypeWriteEffect($"\n[CYBER-SEC v2.077] → {response}", ConsoleColor.Green);
                IsRunning = false;
                return;
            }

            // Check if the input is a basic command
            if (responses.ContainsKey(input))
            {
                TypeWriteEffect($"\n[CYBER-SEC v2.077] → {string.Format(responses[input], UserName)}", ConsoleColor.Green);
                return;
            }

            // Check if the input is a cybersecurity topic
            if (topicContent.ContainsKey(input))
            {
                DisplayTopicContent(input);
                return;
            }

            // If we get here, the input wasn't recognized
            TypeWriteEffect($"\n[CYBER-SEC v2.077] → I'm not sure what you mean by '{input}'. Type 'help' for a list of topics and commands.", ConsoleColor.Green);
        }

        static void DisplayTopicContent(string topic)
        {
            var content = topicContent[topic];

            // Display topic header
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n╔══════════════════ {topic.ToUpper()} ══════════════════╗");
            Console.ResetColor();

            // Display definition
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n📘 Definition:");
            Console.ResetColor();
            TypeWriteEffect($"{content.definition}", ConsoleColor.Yellow);

            // Display example
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n🔍 {topic} Example:");
            Console.ResetColor();
            TypeWriteEffect($"{content.example}", ConsoleColor.Magenta);

            // Display activity
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n⚡ Practice Activity:");
            Console.ResetColor();
            TypeWriteEffect($"{content.activity}", ConsoleColor.Green);

            // Add a prompt for the next topic
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n╚════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            TypeWriteEffect("\n[CYBER-SEC v2.077] → What other cybersecurity topic would you like to explore?", ConsoleColor.Green);
        }

        static void ChatLoop()
        {
            while (IsRunning)
            {
                // Display prompt for user input
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("\n➤ ");
                Console.ForegroundColor = ConsoleColor.White;

                // Get user input
                string input = Console.ReadLine().Trim();
                Console.ResetColor();

                // Process the input
                if (!string.IsNullOrWhiteSpace(input))
                {
                    ProcessInput(input);
                }
            }

            // Display exit message with animated goodbye
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                 TERMINATING SESSION                     ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            // Animated goodbye
            string goodbye = "[SYSTEM SHUTTING DOWN - REMEMBER TO STAY CYBER-SAFE]";
            TypeWriteEffect(goodbye, ConsoleColor.Red);

            // Countdown animation
            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int i = 3; i > 0; i--)
            {
                Console.Write($"\rExiting in {i}...");
                Thread.Sleep(1000);
            }
            Console.WriteLine("\rGoodbye!            ");
            Console.ResetColor();

            // Program will exit naturally as the Main method concludes
        }
    }
}

