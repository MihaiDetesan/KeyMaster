using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MiDe.KeyMaster.Models;
using System;
using System.Data;

namespace MiDe.KeyMaster.App
{

    public class BorrowController
    {
        public event EventHandler ChangeToKey;
        public event EventHandler ChangeToPerson;
        public event EventHandler ClearKey;
        public event EventHandler ClearPerson;
        public event EventHandler<MessageEventArgs> DisplayStatusMessage;

        private BorrowOperations borrowOps;
        private KeyOperations keyOps;
        private PersonOperations personOps;
        private PermissionOperations permissionOps;
        private Db db;
        private ILogger logger;
        Key currentKey;
        Person currentPerson;

        private BorrowState state;

        public BorrowController(string dbPath, ILogger logger)
        {
            var options = new DbOptions()
            {
                DatabaseName = dbPath
            };

            this.logger = logger;

            db = new Db(options, logger);

            keyOps = new KeyOperations(db, logger);
            personOps = new PersonOperations(db, logger);
            borrowOps = new BorrowOperations(db, logger);
            permissionOps = new PermissionOperations(db, logger);
        }

        public BorrowController(string dbPath) : this(dbPath, NullLogger.Instance)
        {
            OnChangeToKey();
        }

        public void AddNewInput(string candidate)
        {
            if (state == BorrowState.Key)
            {
                if (TryAddingANewKey(candidate))
                {
                    state = BorrowState.Person;
                    OnChangeToPerson();
                }
            }
            else
            {
                if (TryAddingANewPerson(candidate))
                {
                    if (!permissionOps.Exists(new Permission()
                    {
                        PersonId = currentPerson.Id,
                        KeyId = currentKey.Id
                    }))
                    {
                        var errorMessage = $"{currentPerson.FirstName} {currentPerson.LastName} nu are dreptul sa imprumute" +
                            $" cheia {currentKey.Description}";
                        OnDisplayStatusMessage(new MessageEventArgs(errorMessage));
                    }
                    else
                    {
                        var newBorrow = new Borrow()
                        {
                            KeyId = currentKey.Id,
                            PersonId = currentPerson.Id
                        };

                        var conflictingBurrow = borrowOps.SearchByKey(newBorrow.KeyId);

                        if (conflictingBurrow is Borrow && conflictingBurrow.PersonId != newBorrow.PersonId)
                        {
                            var loanMessage = $"Cheia {currentKey.Description} e deja imprumutata de {currentPerson.LastName} {currentPerson.FirstName}";
                            OnDisplayStatusMessage(new MessageEventArgs(loanMessage));
                        }
                        else
                        {
                            if (borrowOps.Exists(newBorrow))
                            {
                                var loanMessage = $"{currentPerson.LastName} {currentPerson.FirstName} a inapoiat cheia {currentKey.Description}";
                                OnDisplayStatusMessage(new MessageEventArgs(loanMessage));
                                borrowOps.Delete(newBorrow.KeyId);
                            }
                            else
                            {
                                var loanMessage = $"{currentPerson.LastName} {currentPerson.FirstName} a imprumutat cheia {currentKey.Description}";
                                OnDisplayStatusMessage(new MessageEventArgs(loanMessage));
                                borrowOps.Add(newBorrow);
                            }
                        }
                    }

                    state = BorrowState.Key;
                    OnChangeToKey();
                }
            }
        }

        public bool TryAddingANewKey(string keyCandidate)
        {
            var key = keyOps.Search(keyCandidate);
            if (key is null)
            {
                var missingMessage = $"Nu am gasit cheia cu codul {keyCandidate}.Scaneaza din nou cheia.";
                OnDisplayStatusMessage(new MessageEventArgs(missingMessage));
                OnClearKey();
                return false;
            }
            else
            {
                currentKey = key;
                OnDisplayStatusMessage(new MessageEventArgs($"Ati scanat cheia {key.Description}"));

            }

            return true;
        }

        public bool TryAddingANewPerson(string personCandidate)
        {
            var person = personOps.Search(personCandidate);

            if (person is null)
            {
                var missingMessage = $"Nu am gasit persoana cu codul {personCandidate}.Scaneaza din nou persoana.";
                OnDisplayStatusMessage(new MessageEventArgs(missingMessage));
                OnClearPerson();
                return false;
            }
            else
            {
                currentPerson = person;
                OnDisplayStatusMessage(new MessageEventArgs($"Ati scanat persoana {person.FirstName} {person.LastName}"));
            }

            return true;
        }

        public DataTable? GetLoansByPage(int pageIndex, int pageSize)
        {
            return borrowOps.GetByPage(pageIndex, pageSize);
        }


        protected virtual void OnChangeToKey()
        {
            EventHandler handler = ChangeToKey;
            handler?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void OnChangeToPerson()
        {
            EventHandler handler = ChangeToPerson;
            handler?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnClearKey()
        {
            EventHandler handler = ClearKey;
            handler?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnClearPerson()
        {
            EventHandler handler = ClearPerson;
            handler?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnDisplayStatusMessage(MessageEventArgs e)
        {
            logger.LogDebug(e.Message);
            EventHandler<MessageEventArgs> handler = DisplayStatusMessage;
            handler?.Invoke(this, e);
        }
    }

    public enum BorrowState
    {
        Key,
        Person,
    }

    public class MessageEventArgs : EventArgs
    {
        public MessageEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }

}
