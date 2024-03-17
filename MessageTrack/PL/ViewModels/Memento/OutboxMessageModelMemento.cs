using MessageTrack.PL.Models;

namespace MessageTrack.PL.ViewModels.Memento
{
    public class OutboxMessageModelMemento
    {
        public OutboxMessageModel State { get; private set; }

        public OutboxMessageModelMemento(OutboxMessageModel stateToSave)
        {
            State = stateToSave.Clone() as OutboxMessageModel;
        }
    }
}
