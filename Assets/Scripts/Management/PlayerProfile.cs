public enum InterceptorLookMode { Locked, Free }
public enum MechLookMode { Locked, Free }

//Default for now. To implement later
public class PlayerProfile {

    public string name { get; private set; }

    #region Controls
    private ControlProfile controls;
    #endregion Controls

    #region Interceptor
    public InterceptorLookMode interceptorLookMode { private set; get; }
    #endregion

    #region Mech
    public MechLookMode mechLookMode { private set; get; }
    #endregion

    #region Saved Games
    //TODO Overhaul the Save Game System to allow multiple saves per profile. Allow choosing of save games before hosting
    private SaveGame save;
    #endregion

    private PlayerProfile() {}

    public static PlayerProfile newDefaultPlayerProfile() {
        
        PlayerProfile npp = new PlayerProfile();
        npp.name = "New001";
        npp.controls = new ControlProfile();
        npp.interceptorLookMode = InterceptorLookMode.Free;
        npp.mechLookMode = MechLookMode.Free;

        return npp;
    }
}
