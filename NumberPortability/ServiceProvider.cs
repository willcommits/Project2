using System.Collections;

namespace NumberPortability;

public class ServiceProvider
{
    private String _providername;
    private BitArray vodacom = new BitArray(1000000);
    private BitArray cellc = new BitArray(1000000);
    private BitArray mtn = new BitArray(1000000);
    private BitArray telkom  = new BitArray(1000000);

    public ServiceProvider(String providername)
    {
        _providername = providername;
    }

    public Boolean IspartOfProvider(Int32 NumberSuffix,String OriginalProvider)
    {
        if (OriginalProvider.Equals("TK"))
        {
            if (telkom[NumberSuffix])
            {
                return true;
            }
        } 
        
        if (OriginalProvider.Equals("CC"))
        {
            if (cellc[NumberSuffix])
            {
                return true;
            }
        } 
        
        if (OriginalProvider.Equals("MTN"))
        {
            if (mtn[NumberSuffix])
            {
                return true;
            }
        }
        
        if (OriginalProvider.Equals("VC"))
        {
            if (vodacom[NumberSuffix])
            {
                return true;
            }
        }

        return false;

    }

    public void PopulateValues(Int32 NumberSuffix, String OriginalProvider)
    {
        return NotImple
    }
    
    

}