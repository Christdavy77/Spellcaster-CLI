using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
 
class Program
{
    static async Task Main()
    {
        string openAiKey = "APiKey"; 
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
                model = "gpt-4o-mini",
                max_tokens = 100,
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
 
            Console.WriteLine("Choisissez une langue :");
            Console.WriteLine("1 - Anglais (US)");
            Console.WriteLine("2 - Anglais (UK)");
            Console.WriteLine("3 - Espagnol");
            Console.Write("Votre choix : ");
            string choix = Console.ReadLine();

            string langue = choix switch
            {
                "1" => "en-US",  
                "2" => "en-GB",  
                "3" => "es-ES",  
                _ => "en-US"     
            };

            // Instruction MODIFIÉE pour inclure la langue choisie
            string instruction = $"Tu es un traducteur de langue. " +
                                $"Traduis ce texte en {langue} de manière précise et naturelle, " +
                                "sans ajouter de commentaires.";
            
            var requete = new
            {
                model = "gpt-4o-mini",
                
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

        string instruction = $@"
            Tu es un ChatGPT, donc comporte-toi naturellement, mais tu as la particularité de pouvoir générer des fichiers HTML bien conçus avec un beau rendu, mais cela va dépendre du thème que l'utilisateur va devoir rentrer, et tu as tous les choix du frontend qui te convient et du contenu de ce fichier. je ne veux pas de commentaire de ta part et eviter de me donner un titre html ";
            

 
        var requete = new
        {
            model = "gpt-4o-mini",
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
        }
        Console.Write("\nVoulez-vous une autre génération traduire un autre texte ? (o/n) : ");
        string reponseUtilisateur = Console.ReadLine().ToLower();
        if (reponseUtilisateur != "o") break;
    }
    Console.WriteLine("\n OK à la prochaine chef");
}
}
