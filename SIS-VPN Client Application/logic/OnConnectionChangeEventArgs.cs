namespace SIS_VPN_Client_Application.logic
{
    public class OnConnectionChangeEventArgs
    {
        public bool NewState { get; }

        public OnConnectionChangeEventArgs(bool newState)
        {
            NewState = newState;
        }
    }
}
