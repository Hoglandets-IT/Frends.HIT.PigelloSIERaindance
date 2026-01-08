using jsiSIE;
namespace Frends.HIT.PigelloSIERaindance;

/// <summary>
/// A class for sorting SIE Dimensions a bit quicker
/// </summary>
class VoucherDimensions
{
    private readonly Dictionary<int, string>  _dimensionDict;
    
    public string GetDim(int id) => _dimensionDict.GetValueOrDefault(id, "");
    
    public VoucherDimensions(SieVoucherRow row)
    {
        _dimensionDict = new Dictionary<int, string>();
        foreach (var voucherObject in row.Objects)
        {
            var id = Convert.ToInt32(voucherObject.Dimension.Number);
            var  value = voucherObject.Number;
            _dimensionDict[id] = value;
        }
    }
}