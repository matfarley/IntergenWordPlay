/* Author: Matthew Farley
 * Date: 12 June 2013
 * 
 * This class is the main model/engine for the application and is responsible for all
 * of the data manipulation.  Once the data is processed the methods will either return
 * the answer or make the answer accessable via a getter method.  
 * Exceptions are handled by the calling methods in frmMain.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace WordPlay
{
    class ParsingEngine
    {
        //ArrayLists are used to clean up Arrays
        ArrayList wordList = new ArrayList();
        ArrayList sentenceList = new ArrayList();
        private String[] finalWords;
        private String[] finalSentences;
        private int longestIndex;
        private String mostFrequentWord;
        private String wordLengthOutput;


        public int getLongestIndex()
        {
            return longestIndex;
        }

        public String getMostFrequentWord()
        {
            return mostFrequentWord;
        }

        public String getWordLenghtOutput()
        {
            return wordLengthOutput;
        }
        
        //Takes in a string and returns an array of individual words 
        public String[] CreateWordArray(String text)
        {
            //Words are defined as groups of characters seperated by the following delimiters
            char[] wordDelimiters = { ' ', ',', '.', ':', '!', '?', ';'};
            String[] initialWords = text.Split(wordDelimiters);
            
            /* Known bug, where carriage returns are entered ito the array as words.
             Trying to fix by converting to array list, removing elements then converting back.*/
            foreach (String word in initialWords)
            {
                if (word != "")
                {

                    wordList.Add(word.TrimEnd('?','.',','));
                }
            }
            //trying to clean up words.  Commas etc attached to words are upsetting the count.  Need to remove them.
            for (int i = 0; i < wordList.Count; i++)
            {
                String word = (string)wordList[i];
                word = word.Replace("'s", "");
                word = word.Replace(",", "");
                wordList[i] = word;
            }


            finalWords = (String[]) wordList.ToArray(typeof(String));
            
            return finalWords;
 
        }

        public String[] CreateSentenceArray(String text)
        {
            //Sentences defined as groups of characters delimeted by any of the following characters
            char[] sentenceDelimiters = {'.', '!', '?'};
            String[] initialSentences = text.Split(sentenceDelimiters);

            foreach (String sentence in initialSentences)
            {
                if (sentence != "")
                {
                    sentenceList.Add(sentence);
                }
            }
            finalSentences = (String[])sentenceList.ToArray(typeof(String));
            
            return finalSentences;
        }

        /* Sentence with the most words is defined as the longest element. Sentences in the array are sorted,
            returning the last element*/
        public String FindLongestSentence()
        {
            //THROWS EXCEPTIONS IF NO FILE IS LOADED! Exceptions handled by calling method.
                String longestSentence;
                var orderedSentences = finalSentences.OrderBy(sentence => sentence.Length);
                longestSentence = orderedSentences.Last();
                longestIndex = Array.IndexOf(finalSentences, longestSentence);
                // clean up carriage returns
                longestSentence = longestSentence.Substring(longestSentence.LastIndexOf('\r') + 1);
                longestSentence = longestSentence.Substring(longestSentence.LastIndexOf('\n') + 1);
                return longestSentence;

        }

       
        /*Uses dictionary to find the most frequently used word.  
         * Not real confident with dictionaries, enlisted the internet for help with this.
         */
        public void FindMostFrequentWord()
        {
            Dictionary<string, int> dic = finalWords.Distinct().ToDictionary(p => p, p => finalWords.Count(t => t == p));
            KeyValuePair<String, int> max = new KeyValuePair<String, int>("NONE", 0);
            foreach (KeyValuePair<String, int> kvp in dic)
                if (kvp.Value > max.Value)
                    max = kvp;
           mostFrequentWord = max.Key;
            //Known that it only returns 1 value.  Will fix if have time.
        }

        /*Again not real confident with the List data structure.  Have experienced glitches
         * with other test data but seems to return the correct answer for the given test text.
         * Again I used the internet for help here.
         */
        public void findXXLongestWord()
        {
            if (finalWords.Length > 2)
            {
                List<int> allLengths = finalWords.Select(x => x.Length).Distinct().ToList();

                int thirdLargestWordLength = allLengths.OrderByDescending(x => x).Skip(2).Distinct().Take(1).FirstOrDefault();

                if (finalWords[0].Length != thirdLargestWordLength && finalWords[1].Length != thirdLargestWordLength)
                {
                    string[] theThirdLargestWords = finalWords.Where(x => x.Length == thirdLargestWordLength).ToArray();

                    if (theThirdLargestWords.Length == 1)
                    {
                        wordLengthOutput = theThirdLargestWords[0];
                    }
                    else
                    {
                        string words = "";

                        for (int i = 0; i < theThirdLargestWords.Length; i++)
                        {
                            if (i == 0)
                            {
                                words = theThirdLargestWords[i];
                            }
                           
                            else
                            {
                                words += ", " + theThirdLargestWords[i];
                            }
                        }

                        wordLengthOutput = words;
                       
                    }
                }
            }
            
        }

       
    }
}
    
