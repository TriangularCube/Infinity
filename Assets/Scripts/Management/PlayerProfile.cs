public enum InterceptorLookMode { Locked, Free }
public enum MechLookMode { Locked, Free }

//Default for now. To implement later
public class PlayerProfile {

    private string name;

    #region Controls
    private ControlProfile controls;
    #endregion Controls

    #region Interceptor
    public InterceptorLookMode interceptorLookMode { private set; get; }
    #endregion

    #region Mech
    public MechLookMode mechLookMode { private set; get; }
    #endregion

    private PlayerProfile() {}

    public static PlayerProfile newPlayerProfile() {
        
        PlayerProfile npp = new PlayerProfile();
        npp.name = "New001";
        npp.controls = new ControlProfile();
        npp.interceptorLookMode = InterceptorLookMode.Free;
        npp.mechLookMode = MechLookMode.Free;

        return npp;
    }
}
