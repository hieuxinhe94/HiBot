using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Scorables.Internals;
using Microsoft.Bot.Connector;

namespace HiBot.Dialogs
{
    [Serializable]
    public class HelpDialog : ScorableBase<IActivity, string, double>
    {
        /// <summary>
        ///  This dialog will be handler global message, reply any thing will be coming last dialog.
        /// </summary>

        private readonly IDialogTask task;

        public HelpDialog(IDialogTask task)
        {
            SetField.NotNull(out this.task, nameof(task), task);
        }
        protected override Task DoneAsync(IActivity item, string state, CancellationToken token)
        {
            throw new NotImplementedException();
            /*
             * DoneAsync is called after the scoring process is complete. Use this method to dispose of any scoped resources.
             */
        }

        protected override Task<string> PrepareAsync(IActivity item, CancellationToken token)
        {
            /*
             * PrepareAsync is the first method that is called in the scorable instance.
             * It accepts incoming message activity, analyzes and sets the dialog's state, which is passed to all the other methods of the IScorable interface.
             */
            throw new NotImplementedException();
        }

        protected override bool HasScore(IActivity item, string state)
        {
            /*
             * The HasScore method checks the state property to determine if the scorable dialog should provide a score for the message.
             * If it returns false, the message will be ignored by the scorable dialog.
             */
            throw new NotImplementedException();
        }

        protected override double GetScore(IActivity item, string state)
        {
            throw new NotImplementedException();
            /*
             * GetScore will only trigger if HasScore returns true.
             * You’ll provision the logic in this method to determine the score for a message between 0 - 1
             */
        }

        protected override Task PostAsync(IActivity item, string state, CancellationToken token)
        {
            throw new NotImplementedException();
            /*
             * In the PostAsync method, define core actions to be performed for the scorable class
             * . All scorable dialogs will monitor incoming messages, and assign scores to valid messages based on the scorables' GetScore method. The scorable class which determines the highest score (between 0 - 1.0) will then trigger that scorable's PostAsync method.
             */
        }
    }
}