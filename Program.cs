using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
 
class Program
{
    static async Task Main()
    {
        string openAiKey = "openAiKey"; 
        string apiUrl = "https://api.openai.com/v1/chat/completions";
 
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiKey}");
 
        while (true)
        {
            Console.WriteLine("\nSpellcaster CLI");

            Console.WriteLine("\nCorrecteur d'orthographe 📝");

            Console.Write("Tape une phrase à corriger : ");
            string texteUtilisateur = Console.ReadLine();
 
            var requete = new
            {
                model = "gpt-3.5-turbo",
                max_token = 100,
                messages = new[]
                {
                    new { role = "system", content = "Tu es un correcteur d'orthographe. Corrige les fautes du texte sans ajouter d'explication." },
                    new { role = "user", content = texteUtilisateur }
                }
            };
 
            string jsonRequete = JsonSerializer.Serialize(requete);
            var contenu = new StringContent(jsonRequete, Encoding.UTF8, "application/json");
 
            HttpResponseMessage reponse = await httpClient.PostAsync(apiUrl, contenu);
            string json = await reponse.Content.ReadAsStringAsync();
 
            if (reponse.IsSuccessStatusCode)
            {
                JsonDocument doc = JsonDocument.Parse(json);
                string reponseCorrigee = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();
 
                Console.WriteLine("\nTexte corrigé :");
                Console.WriteLine(reponseCorrigee);
            }
            else
            {
                Console.WriteLine($"Erreur API : {reponse.StatusCode}");
            }
 
            Console.Write("\nVoulez-vous corriger un autre texte ? (o/n) : ");
            string reponseUtilisateur = Console.ReadLine().ToLower();
            if (reponseUtilisateur != "o") break;
 
        }
            
            while (true)
        {
            Console.WriteLine("\nTraduction");
            Console.Write("\nTexte à traduire : ");
            string texteUtilisateur = Console.ReadLine();
 
            string instruction = @"Tu es un traducteur";
 
            Console.WriteLine("Choisissez une langue :");
            Console.WriteLine("1 - Anglais (US)");
            Console.WriteLine("2 - Anglais (UK)");
            Console.WriteLine("3 - Espagnol");
            Console.Write("Votre choix : ");
            string choix = Console.ReadLine();
 
            string langue = choix switch
            {
                "1" => "Anglais (US)",
                "2" => "Anglais (UK)",
                "3" => "Espagnol",
            };
 
            var requete = new
            {
                model = "gpt-3.5-turbo",
                max_token = 200,
                messages = new[]
                {
                    new { role = "system", content = instruction },
                    new { role = "user", content = texteUtilisateur }
                }
            };
 
            string jsonRequete = JsonSerializer.Serialize(requete);
            var contenu = new StringContent(jsonRequete, Encoding.UTF8, "application/json");
 
            HttpResponseMessage reponse = await httpClient.PostAsync(apiUrl, contenu);
            string json = await reponse.Content.ReadAsStringAsync();
 
            if (reponse.IsSuccessStatusCode)
            {
                JsonDocument doc = JsonDocument.Parse(json);
                string traduction = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();
 
                Console.WriteLine("\nTraductions :");
                Console.WriteLine(traduction);
            }
            else
            {
                Console.WriteLine($"Erreur API : {reponse.StatusCode}");
            }
 
            Console.Write("\nVoulez-vous traduire un autre texte ? (o/n) : ");
            string reponseUtilisateur = Console.ReadLine().ToLower();
            if (reponseUtilisateur != "o") break;
 
            Console.WriteLine();
        }
        
        while(true)
    {
        Console.WriteLine("\nGenerateur de HTML");

        Console.Write("Titre de l'article : ");
        string titre = Console.ReadLine();

        string instruction = @"Tu es un générateur de page web.
        Crée une page HTML complète, simple et bien structurée pour un article selon le titre de l'article entrer par l'utilisateur.Affiche le titre dans un <h1>, le contenu dans un <p>. Mets un style CSS minimal pour rendre le tout lisible.";
 
        var requete = new
        {
            model = "gpt-3.5-turbo",
            max_token = 1000,
            messages = new[]
            {
                new { role = "system", content = instruction },
                new { role = "user", content = $"Titre: {titre}" }
            }
        };
 
        string jsonRequete = JsonSerializer.Serialize(requete);
        var contenuHttp = new StringContent(jsonRequete, Encoding.UTF8, "application/json");
 
        HttpResponseMessage reponse = await httpClient.PostAsync(apiUrl, contenuHttp);
        string json = await reponse.Content.ReadAsStringAsync();
 
        if (reponse.IsSuccessStatusCode)
        {
            JsonDocument doc = JsonDocument.Parse(json);
            string html = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();
 
            string cheminFichier = Path.Combine(Directory.GetCurrentDirectory(), "article.html");
            File.WriteAllText(cheminFichier, html);
 
            Console.WriteLine($"Fichier HTML généré avec succès : {cheminFichier}");
        }
        else
        {
            Console.WriteLine($" Erreur API : {reponse.StatusCode}");
            Console.WriteLine(json);

            Console.Write("\nVoulez-vous traduire un autre texte ? (o/n) : ");
            string reponseUtilisateur = Console.ReadLine().ToLower();
            if (reponseUtilisateur != "o") break;
        }
        
    }
    Console.WriteLine("\n OK à la prochaine chef");
}
}
    
 
 