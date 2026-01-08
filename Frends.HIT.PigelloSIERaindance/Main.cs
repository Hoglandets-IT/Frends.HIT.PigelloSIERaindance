using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using jsiSIE;


namespace Frends.HIT.PigelloSIERaindance;

class Main
{
    /// <summary>
    /// This is the information shown about the task in the Frends control panel.
    /// The TaskInput input parameters will be shown on a different tab than the verbose option with the PropertyTab attribute.
    /// </summary> 
    /// <param name="verbose">This parameter increases the verbosity of the output</param>
    /// <param name="input">A set of parameters for doing something in the class</param>
    /// <returns>TaskOutput object</returns>
    public static TaskOutput ParsePigelloSIE(bool verbose, [PropertyTab] TaskInput input)
    {
      Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
   

      //const string fileName = @"/Users/ellie/Documents/Projects/PigelloSieConverter/Docs/MA_Industrifastigheter_AB_20250919.sie";

      var sieDoc = new SieDocument()
      {
          ThrowErrors = false, 
          IgnoreMissingOMFATTNING = true,
          Encoding = Encoding.GetEncoding(437)
      };
      var sieData = new MemoryStream(input.File);
      sieDoc.ReadDocument(sieData);

      var vers = sieDoc.VER;

      /*
       * H 230402 Huvudtext h�r
       * K konto     ansvar    vht       akt       obj       proj      fri       motp                                                           50000 Radtext rad 1                    PerNyc 230101230201   
       * K konto     ansvar    vht       akt       obj       proj      fri       motp                                                -          50000 Radtext rad 2                    PerNyc 230101230201   
       */
      var buf = "";
      foreach (var ver in vers)
      {
          buf += Helpers.GetHeaderLine(ver.VoucherDate, ver.Text);


          buf = (from v in ver.Rows 
              let dimensions = new VoucherDimensions(v) 
              select Helpers.GetTransactionLine(konto: v.Account.Number, 
                                                ansvar: dimensions.GetDim(1), 
                                                objekt: dimensions.GetDim(2), 
                                                motpart: dimensions.GetDim(3), 
                                                källa: "EK23", radbelopp: 
                                                v.Amount, text: dimensions.GetDim(4)))
              .Aggregate(buf, (current, voucherResult) => current + voucherResult);
      }

      byte[] returnByteStream = Encoding.ASCII.GetBytes(buf);

      return new TaskOutput(returnByteStream, "Success");
    }
}