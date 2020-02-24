using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace InvertedIndex
{

    public static class Extensions
    {
        public static IEnumerable<string> And (this Dictionary<string, List<string>> index, string firstTerm, string secondTerm)
        {
            return(from d in index
                         where d.Key.Equals(firstTerm)
                         select d.Value).SelectMany(x=>x).Intersect
                            ((from d in index
                             where d.Key.Equals(secondTerm)
                             select d.Value).SelectMany(x => x));
        }

        public static IEnumerable<string> Or(this Dictionary<string, List<string>> index, string firstTerm, string secondTerm)
        {
            //return (from d in index
            //        where d.Key.Equals(firstTerm)
            //        select d.Value).SelectMany(x => x).ToList().Union
            //                ((from d in index
            //                  where d.Key.Equals(secondTerm)
            //                  select d.Value).SelectMany(x => x).ToList()).Distinct();

            return (from d in index
                        where d.Key.Equals(firstTerm) || d.Key.Equals(secondTerm)
                        select d.Value).SelectMany(x=>x).Distinct();
           
        }


        class EntryPoint
        {


            static void Main(string[] args)
            {
                //    //invertedIndex = new Dictionary<string, List<string>>();
                //    //string folder = "C:\\Users\\Elena\\Documents\\Visual Studio 2013\\Projects\\InvertedIndex\\Files\\";

                //    foreach (string file in Directory.EnumerateFiles(folder, "*.txt"))
                //    {
                //        List<string> content = System.IO.File.ReadAllText(file).Split(' ').Distinct().ToList();
                //        addToIndex(content, file.Replace(folder, ""));
                //    }

                List<string> ngramList = new List<string>();
                Dictionary<string, List<string>> invertedIndex = new Dictionary<string, List<string>>();
                string[] texts = {"i am so bad boy i hate my self mukesh fuck ","i am Paragraphs are the building blocks of papers. Many students define paragraphs in terms of length:" +
                " a paragraph is a group of at least five sentences, a paragraph is half a page long, etc. In reality, though, the unity and " +
                "coherence of ideas among sentences is what constitutes a paragraph. A paragraph is defined as “a group of sentences or a single sentence that forms a unit” " +
                "(Lunsford and Connors 116). Length and appearance do not determine fuck whether a section in a paper is a paragraph. " +
                "For instance, in some styles of writing, particularly journalistic styles, a paragraph can be just one sentence long. mukesh Ultimately," +
                " a paragraph is a sentence or group of sentences that support one main idea.","Nepal is very mountainous and hilly. Roughly rectangular in" +
                " shape, about 650 kilometer long and about 200 kilometer wide, Nepal is the third biggest country" +
                " in South Asia, with an area of 147,181 square kilometer of land. Nepal is fuck a land-locked country, surrounded by India on three" +
                " sides and by China’s Xizang Autonomous Region (Tibet) to the north.","A paragraph is a series of sentences that are organized and coherent, and are all related to a single topic." +
                " Almost every piece of writing you do that is longer than a few sentences should be organized into paragraphs.","Welcome to Shortparagraph.com! Our mission is to provide an online platform to help students" +
                " to discuss anything and everything about Paragraph. This website includes study notes, research papers, essays, articles and other allied information submitted by visitors like YOU."};
                char[] panchutation = { '[', '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '+', '=',
                '{', '[', '}', ']', '|', ',', ',', ',', '<', '.', '>', '/', '?', ']', '"', ':', '“', '”' };
                foreach (var item in texts)
                {
                    List<string> content = item.Trim(panchutation).Split(' ').Distinct().ToList();
                    //addToIndex(content, item, invertedIndex);
                    unigrams(content, panchutation, ngramList);
                    bigrams(content, panchutation, ngramList);

                    Ngrams(content, panchutation,ngramList);
                   

                }
                addToIndex(ngramList, texts, invertedIndex);
                var resAnd = invertedIndex.And("star", "sparkling");
                var resOr = invertedIndex.Or("star", "sparkling");
                string keyword = Console.ReadLine();
                Searching(invertedIndex,keyword);
                Console.ReadLine();
            }
            public static void bigrams(List<string> tokens, char[] panchutation, List<string> ngramList)
            {

                for (int k = 0; k < (tokens.Count - 2 + 1); k++)
                {

                    String s = "".Trim(panchutation);
                    int start = k;
                    int end = k + 2;
                    for (int j = start; j < end; j++)
                    {
                        s = s + " " + tokens[j];
                    }
                    //Add n-gram to a list

                    ngramList.Add(s.ToLower());



                }


            }
            public static void unigrams(List<string> tokens, char[] panchutation, List<string> ngramList)
            {

                for (int k = 0; k < (tokens.Count - 1 + 1); k++)
                {

                    String s = "".Trim(panchutation);
                    int start = k;
                    int end = k + 1;
                    for (int j = start; j < end; j++)
                    {
                        s = s + " " + tokens[j];
                    }
                    //Add n-gram to a list

                    ngramList.Add(s.ToLower());



                }

            }
            public static void Ngrams(List<string> tokens, char[] panchutation, List<string> ngramList)
            {
                for (int k = 0; k < (tokens.Count - 3 + 1); k++)
                {

                    String s = "".Trim(panchutation);
                    int start = k;
                    int end = k + 3;
                    for (int j = start; j < end; j++)
                    {
                        s = s + " " + tokens[j];
                    }
                    //Add n-gram to a list

                    ngramList.Add(s.ToLower());
                 


                }
            }
            public static void Searching(Dictionary<string, List<string>> index, string keyword)
            {


                try
                {
                    var value = index[keyword];

                    if (index.TryGetValue(keyword, out value))
                    {
                        foreach (string document in index[keyword])
                        {

                            Console.WriteLine(document);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Not found!");
                    }



                    Console.ReadKey(true);
                }
                catch
                {
                    Console.WriteLine("enter valid words");
                }





            }

            private static void addToIndex(List<string> words, string[] documents, Dictionary<string, List<string>> invertedIndex)
            {
                foreach (var document in documents)
                {
                    foreach (var word in words)
                    {
                        Console.WriteLine(word);
                        if (!invertedIndex.ContainsKey(word))
                        {
                            
                            
                         invertedIndex.Add(word, new List<string> { document });
                            
                        }
                        else
                        {
                            invertedIndex[word].Add(document);
                        }
                        
                       
                    }

                }
               

            }
        }
    }
}
