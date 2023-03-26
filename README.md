# LexicalSimilarity
检测英语单词之间的形似度，汉语中有形似字，但是英语中没有形似词的改变，为了便于学习，特地做了个检测单词形似度的程序
我这里用了本地数据库，里面选择的单词库为COCA 5000词、CET-4所有单词和CET-6所有单词，如果想考其他的，可以自己在数据库中增加单词。
我原本使用了求两个单词余弦值的方式求相似度，但是最大的问题是它忽略了字母的顺序，导致差错率非常高，比如计算的结果显示：
与 eloquence相 似 的 单 词 为 :consequence(0.9015),
与 assimilate相 似 的 单 词 为 :materialism(0.9095),stabilise(0.9014),
因此我做了下改进
