using System;
using MySql.Data.MySqlClient;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        
        string connectionString = "server=localhost;port=3306;database=English;uid=***;password=******";
        var wordList = new List<string>();
        string newFilePath = "newfile.txt";
        using (var sw = new System.IO.StreamWriter(newFilePath,true))
        {

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT word FROM OxfordWords";
                MySqlCommand command = new MySqlCommand(query, connection);

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string word = reader.GetString(0);
                    wordList.Add(word);
                }

                reader.Close();
            }

            foreach (var word in wordList)
            {
                var s = "";
                bool isEmpty = true;
                s = String.Format("与{0}相似的单词为:", word);
                foreach (var compareWord in wordList)
                {
                    if (word == compareWord) continue;
                    var r = compareSimilarityByOrder(word, compareWord);
                    //这个数值可以更改，个人感觉0.5比较合适
                    if (r >= 0.5)
                    {
                        s = String.Format("{0}{1}({2:0.0000}),", s, compareWord, r);
                        isEmpty = false;
                    }
                }
                if (!isEmpty)
                {
                    Console.Write(s + "\n");
                    sw.WriteLine(s);

                }

            }

        }
        Console.ReadKey();
        

    }


    /// <summary>
    /// 用次序方式比较两个单词的相似度
    /// </summary>
    /// <param name="word1"></param>
    /// <param name="word2"></param>
    /// <returns></returns>
    static double compareSimilarityByOrder(string word1, string word2)
    {
        
        var shortWord = String.Empty;
        var longWord = String.Empty;
        if (word1.Length<word2.Length)
        {
            shortWord = word1;
            longWord = word2;
        }else
        {
            shortWord = word2;
            longWord = word1;
        }
        if (shortWord.Length < 3) return 0;
        var charArray = shortWord.ToCharArray();
        var strList = new List<string>();
        for (int i = 0; i < charArray.Length-1; i++)
        {
            char[] temp = { charArray[i], charArray[i + 1] };
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(temp);
            if(!strList.Contains(sb.ToString())) strList.Add(sb.ToString());
        }
        int k = 0;
        foreach (var str in strList)
        {
            if (longWord.Contains(str)) k++;
        }

        return (double)k/(double)longWord.Length;
        
    }

    /// <summary>
    /// 用余弦方式比较相似度
    /// </summary>
    /// <param name="word1">第一个单词</param>
    /// <param name="word2">第二个单词</param>
    /// <returns></returns>
    static double compareSimilarity(string word1, string word2)
    {
        // 定义两个向量，每个向量都有 26 个参数
        int[] vector1 = new int[26];
        int[] vector2 = new int[26];

        foreach (var cha in word1)
        {
            if (cha > 122 || cha < 97) continue;
            vector1[cha - 97] = vector1[cha - 97] + 1;
        }
        foreach (var cha in word2)
        {
            if (cha > 122 || cha < 97) continue;
            vector2[cha - 97] = vector2[cha - 97] + 1;
        }

        var cosine = getCos(vector1, vector2);
        return cosine;
        //Console.WriteLine($"两个向量的余弦值为：{cosine}");
    }
    /// <summary>
    /// 计算两个向量的余弦值
    /// </summary>
    /// <param name="vector1">第一个向量</param>
    /// <param name="vector2">第二个向量</param>
    /// <returns></returns>
    static double getCos(int[] vector1, int[] vector2)
    {
        // 计算向量的点积
        double dotProduct = 0;
        for (int i = 0; i < 26; i++)
        {
            dotProduct += vector1[i] * vector2[i];
        }

        // 计算向量的长度
        double length1 = 0;
        double length2 = 0;
        for (int i = 0; i < 26; i++)
        {
            length1 += vector1[i] * vector1[i];
            length2 += vector2[i] * vector2[i];
        }
        length1 = Math.Sqrt(length1);
        length2 = Math.Sqrt(length2);

        // 计算余弦值
        double cosine = dotProduct / (length1 * length2);

        return cosine;
    }

}
