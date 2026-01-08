using System.Globalization;
using jsiSIE;

namespace Frends.HIT.PigelloSIERaindance;



/// <summary>
/// A class containing functions that can be used in the main class for repeated tasks
/// </summary>
class Helpers
{  
    /// <summary>
    /// Truncates a string correctly to fit inside a Raindance field.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="maxLength"></param>
    /// <returns></returns>
    private static string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value[..maxLength];
    }

    /// <summary>
    /// Takes a string like "Hyresfordran (112449523029)" and  rewrites it to "112449523029 Hyresfordran"
    /// </summary>
    /// <param name="s"></param>
    /// <returns>String</returns>
    static string MoveTrailingParenGroupToFront(string s)
    {
        if (string.IsNullOrEmpty(s)) return s;

        int open = s.LastIndexOf('(');
        if (open < 0) return s;

        int close = s.IndexOf(')', open);
        if (close < 0) return s;

        // Only move if the "(...)" is at the end (ignoring trailing whitespace)
        int end = s.AsSpan().TrimEnd().Length - 1;
        if (close != end) return s;

        var prefix = s.AsSpan(0, open).TrimEnd();

        // Content inside the parentheses (strip '(' and ')')
        var inner = s.AsSpan(open + 1, close - open - 1).Trim();

        if (prefix.Length == 0) return s; // nothing to move in front of
        if (inner.Length == 0) return prefix.ToString(); // "(   )" at end -> just drop it

        return string.Concat(inner, " ", prefix);
    }


    /// <summary>
    /// Formats the header line and outputs something like "H 251205 112449523029 Hyresfordran     "
    /// </summary>
    /// <param name="date"></param>
    /// <param name="filemark"></param>
    /// <returns></returns>
    /// 
    public static string GetHeaderLine(DateTime date, string filemark)
    {
        var type = "H";
        var formattedDate = date.ToString("yyMMdd");
        var formattedHeaderText = MoveTrailingParenGroupToFront(filemark);

        //H 251205 Hyresfordran (112449522427) 
        return $"{type,-2}" +
               $"{formattedDate,-7}" +
               $"{Truncate(formattedHeaderText, 30),-30}\r\n";
    }

    /// <summary>
    /// Take input data and formats it to a proper K raindance line like "K 1510      9997                          1110                          870       EK23                                       3240.00         Jamo Industrilack AB                                                                  "
    /// The spaces at the end matter.
    /// </summary>
    /// <param name="konto"></param>
    /// <param name="ansvar"></param>
    /// <param name="verksamhet"></param>
    /// <param name="aktivitet"></param>
    /// <param name="objekt"></param>
    /// <param name="projekt"></param>
    /// <param name="fri"></param>
    /// <param name="motpart"></param>
    /// <param name="källa"></param>
    /// <param name="koddel10"></param>
    /// <param name="koddel11"></param>
    /// <param name="koddel12"></param>
    /// <param name="radbelopp"></param>
    /// <param name="text"></param>
    /// <param name="periodiseringsnyckel"></param>
    /// <param name="periodiseringsdatum1"></param>
    /// <param name="periodiseringsdatum2"></param>
    /// <param name="övrigt"></param>
    /// <returns></returns>
    public static string GetTransactionLine(
    string konto = "",
    string ansvar = "",
    string verksamhet = "",
    string aktivitet = "",
    string objekt = "",
    string projekt = "",
    string fri = "",
    string motpart = "",
    string källa = "",
    string koddel10 = "",
    string koddel11 = "",
    string koddel12 = "",
    decimal radbelopp = 0,
    string text = "",
    string periodiseringsnyckel = "",
    string periodiseringsdatum1 = "",
    string periodiseringsdatum2 = "",
    string övrigt = ""
    )
    {
        var postmarkering = "K";

        string radbelopp1 = radbelopp.ToString(" 0.00;-0.00", CultureInfo.InvariantCulture);

        return
              $"{Truncate(postmarkering, 1),        -2}"                // 1–2
            + $"{Truncate(konto, 10),               -10}"               // 3–12
            + $"{Truncate(ansvar, 10),              -10}"               // 13–22
            + $"{Truncate(verksamhet, 10),          -10}"               // 23–32
            + $"{Truncate(aktivitet, 10),           -10}"               // 33–42
            + $"{Truncate(objekt, 10),              -10}"               // 43–52
            + $"{Truncate(projekt, 10),             -10}"               // 53–62
            + $"{Truncate(fri, 10),                 -10}"               // 63–72
            + $"{Truncate(motpart, 10),             -10}"               // 73–82
            + $"{Truncate(källa, 10),               -10}"               // 83–92
            + $"{Truncate(koddel10, 10),            -10}"               // 93–102
            + $"{Truncate(koddel11, 10),            -10}"               // 103–112
            + $"{Truncate(koddel12, 10),            -12}"               // 113–124
            + $"{radbelopp1,                        -17}"               // 125–141
            + $"{Truncate(text, 30),                -33}"               // 142–174
            + $"{periodiseringsnyckel,              -7}"                // 175–181
            + $"{periodiseringsdatum1,              -6}"                // 182–187
            + $"{periodiseringsdatum2,              -6}"                // 188–193
            + $"{Truncate(övrigt, 166),             -166}"              // 194–359
            + "\r\n";
    }

}