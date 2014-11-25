using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Netplayer = TNet.Player;

#region Setup
public delegate bool DelegateEventHandler( IEvent evt );

public interface IEvent{
	string GetName();
}

public class BaseEvent : IEvent {
	public string GetName(){ return GetType ().Name; }
}
#endregion

#region Events
public class EnteringDockingRange : BaseEvent {
	
	public Terminal terminal{ get; private set; }
	public Carrier carrier{ get; private set; }
	
	public EnteringDockingRange( Terminal toBeDocked, Carrier targetCarrier ){
		terminal = toBeDocked;
		carrier = targetCarrier;
	}
	
}

public class LeavingDockingRange : BaseEvent {
	
	public Terminal terminal{ get; private set; }
	
	public LeavingDockingRange( Terminal toLeave ){
		terminal = toLeave;
	}
	
}

public class RequestLaunch : BaseEvent {

	public Terminal terminal{ get; private set; }
	public Carrier carrier{ get; private set; }

	public RequestLaunch( Terminal toLaunch, Carrier sourceCarrier ){
		terminal = toLaunch;
		carrier = sourceCarrier;
	}

}

public class AllyDocked : BaseEvent {

	public Terminal terminal{ get; private set; }
	public Carrier carrier{ get; private set; }
	
	public AllyDocked( Terminal toDock, Carrier targetCarrier ){
		terminal = toDock;
		carrier = targetCarrier;
	}

}

public class AllyLaunched : BaseEvent {

	public Terminal terminal{ get; private set; }
	public Carrier carrier{ get; private set; }
	
	public AllyLaunched( Terminal toLaunch, Carrier targetCarrier ){
		terminal = toLaunch;
		carrier = targetCarrier;
	}

}

public class AssignedShipRole : BaseEvent {

	public Ship ship{ get; private set; }
	public TNet.Player player{ get; private set; }
	public string role{ get; private set; }

	public AssignedShipRole( Ship onShip, TNet.Player onPlayer, string theRole ){
		ship = onShip;
		player = onPlayer;
		role = theRole;
	}

}
#endregion
	
public class EventManager : Singleton<EventManager> {

	[SerializeField]
	private bool LimitQueueProcesing = false;
	[SerializeField]
	private float QueueProcessTime = 0f;

	private Dictionary<string, ArrayList> m_listenerTable = new Dictionary<string, ArrayList>();
	private Queue<IEvent> eventQueue = new Queue<IEvent>();

	//Add a listener to the event manager that will receive any events of the supplied event name.
	public bool AddListener(string eventName, DelegateEventHandler handler){

		if ( eventName == null ){
			Debug.Log("Event Manager: AddListener failed due to no listener or event name specified.");
			return false;
		}
		
		if (!m_listenerTable.ContainsKey(eventName))
			m_listenerTable.Add(eventName, new ArrayList());

		ArrayList listenerList = m_listenerTable[eventName];
		if (listenerList.Contains(handler)){
			Debug.Log("Event Manager: Listener: " + handler.GetType().Name + " is already in list for event: " + eventName);
			return false; //listener already in list
		}
		
		listenerList.Add(handler);
		return true;
	}

	//Remove a listener from the subscribed to event.
	public bool DetachListener(string eventName, DelegateEventHandler handler){

		if (!m_listenerTable.ContainsKey(eventName))
			return false;
		
		ArrayList listenerList = m_listenerTable[eventName];
		if (!listenerList.Contains(handler))
			return false;
		
		listenerList.Remove(handler);
		return true;
	}

	//Remove a listener from all events
	public void DetachListenerFromAll( DelegateEventHandler handler ){

		foreach (ArrayList list in m_listenerTable.Values) {

			if( list.Contains( handler ) )
			   list.Remove( handler );

		}

	}

	//Inserts the event into the current queue.
	public bool QueueEvent(IEvent evt){
		if (!m_listenerTable.ContainsKey(evt.GetName())){

			Debug.Log("EventManager: QueueEvent failed due to no listeners for event: " + evt.GetName());
			return false;
		
		}
		
		eventQueue.Enqueue(evt);
		return true;
	}

	//Trigger the event instantly, this should only be used in specific circumstances,
	//the QueueEvent function is usually fast enough for the vast majority of uses.
	public bool TriggerEvent(IEvent evt){

		string eventName = evt.GetName();
		if (!m_listenerTable.ContainsKey(eventName)){

			Debug.Log("Event Manager: Event \"" + eventName + "\" triggered has no listeners!");
			return false; //No listeners for event so ignore it
		
		}
		
		ArrayList listenerList = m_listenerTable[eventName];
		foreach (DelegateEventHandler listener in listenerList){

			if (listener(evt))
				return true; //Event consumed.
		
		}
		
		return true;
	}

	//Every update cycle the queue is processed, if the queue processing is limited,
	//a maximum processing time per update can be set after which the events will have
	//to be processed next update loop.
	void Update(){
		float timer = 0.0f;
		while ( eventQueue.Count > 0 ){

			IEvent evt = eventQueue.Dequeue();
			if (!TriggerEvent(evt))
				Debug.Log("Error when processing event: " + evt.GetName());

			if (LimitQueueProcesing)
			{
				timer += Time.deltaTime;

				if (timer > QueueProcessTime)
					return;
			}
		}
	}

	public void OnApplicationQuit(){

		m_listenerTable.Clear();
		eventQueue.Clear();
		_instance = null;
	
	}
}