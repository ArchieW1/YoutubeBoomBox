using UnityEngine;

namespace YoutubeBoomBox.Commands;

public abstract class CommandBase : MonoBehaviour
{
    private string _lastMessage = string.Empty;
    protected abstract string Key { get; }
    
    private void Update()
    {
        string lastChatMessage = HUDManager.Instance.lastChatMessage;
        if (lastChatMessage.StartsWith(Key) && _lastMessage != lastChatMessage)
        {
            Action(lastChatMessage, HUDManager.Instance);
            _lastMessage = lastChatMessage;
        }
    }

    protected abstract void Action(string lastChatMessage, HUDManager instance);
}