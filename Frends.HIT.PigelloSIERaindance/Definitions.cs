using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

// This should be the root namespace of the package
namespace Frends.HIT.PigelloSIERaindance;



/// <summary>
/// This is the information shown about the input group in the control panel
/// Since this is set via the interface and not programmatically, it doesn't need a public class for instantiating.
/// </summary>
public class TaskInput
{
    /// <summary>
    /// The source SIE file as a byte array
    /// </summary>
    [DefaultValue("")]
    [DisplayFormat(DataFormatString = "Text")]
    public byte[] File { get; set; }

    /// <summary>
    /// If true the parser will not flag a missing #OMFATTN as an error
    /// </summary>
    //[DisplayFormat(DataFormatString = "Boolean")]
    [DefaultValue(false)]
    public bool IgnoreMissingOMFATTNING { get; set; }

    /// <summary>
    /// If true #BTRANS (removed voucher rows) will be ignored
    /// </summary>
    //[DisplayFormat(DataFormatString = "Boolean")]
    [DefaultValue(false)]
    public bool IgnoreBTRANS { get; set; }

    /// <summary>
    /// If true #RTRANS (added voucher rows) will be ignored
    /// </summary>
    //[DisplayFormat(DataFormatString = "Boolean")]
    [DefaultValue(false)]
    public bool IgnoreRTRANS { get; set; }

    /// <summary>
    /// If true some errors for missing dates will be ignored
    /// </summary>
    //[DisplayFormat(DataFormatString = "Boolean")]
    [DefaultValue(true)]
    public bool IgnoreMissingDate { get; set; }

    /// <summary>
    /// If true don't store values internally. The user has to use the Callback class to get the values. Usefull for large files
    /// </summary>
    //[DisplayFormat(DataFormatString = "Boolean")]
    [DefaultValue(false)]
    public bool StreamValues { get; set; }

    /// <summary>
    /// If false then cache all Exceptions in SieDocument.ValidationExceptions
    /// </summary>
    //[DisplayFormat(DataFormatString = "Boolean")]
    [DefaultValue(true)]
    public bool ThrowErrors { get; set; }

    /// <summary>
    /// The standard says yyyyMMdd and parser will default to that, but you can change the format to whatever you want
    /// </summary>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("yyMMdd")]
    public string DateFormat { get; set; }
    /// <summary>
    /// The standard says codepage 437
    /// </summary>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("437")]
    public string Encoding { get; set; }  
}

/// <summary>
/// This is the output from the task.
/// Since this will be defined programmatically, there is an upside to using
/// a public facing class for creating an instance for the output. 
/// </summary>
public class TaskOutput
{
    /// <summary>
    /// The result of the task.
    /// </summary>
    public byte[] Result { get; set; }
    
    /// <summary>
    /// Any messages returned by the task
    /// </summary>
    public string Info { get; set; }

    public TaskOutput(
        byte[] result,
        string info
    )
    {
        Result = result;
        Info = info;
    }
}