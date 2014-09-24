using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSharp
{
    
    public class ProConstructor
    {

        protected static readonly object[] myEmptyArray;

        protected ProObject myPrototype;

        protected Action<dynamic, object[]> myConstructorAction;

        static ProConstructor()
        {

            myEmptyArray = new object[0];

        }

        public ProConstructor()
        {

            myConstructorAction = (TheObject, ThePrameters) =>
            {
            };

        }

        public ProConstructor(Action<dynamic, object[]> TheConstructorAction)
        {

            myConstructorAction = TheConstructorAction;

        }

        public ProObject Prototype
        {

            get
            {

                return myPrototype;

            }
            set
            {

                myPrototype = value;

            }

        }

        public bool HasPrototype
        {

            get
            {

                return myPrototype != null;

            }

        }

        public Action<dynamic, object[]> ConstructorAction
        {

            get
            {

                return myConstructorAction;

            }
            set
            {

                if(value == null)
                {

                    myConstructorAction = (TheObject, ThePrameters) =>
                    {
                    };

                    return;

                }

                myConstructorAction = value;

            }

        }

        public void ResetConstructorAction()
        {

            myConstructorAction = (TheObject, ThePrameters) =>
            {
            };

        }

        public ProObject New()
        {

            ProObject Obj;

            if(myPrototype != null)
                Obj = new ProObject(myPrototype);
            else
                Obj = new ProObject();

            myConstructorAction(Obj, myEmptyArray);

            return Obj;

        }

        public ProObject New(params object[] ThePrameters)
        {

            ProObject Obj;

            if(myPrototype != null)
                Obj = new ProObject(myPrototype);
            else
                Obj = new ProObject();

            myConstructorAction(Obj, ThePrameters);

            return Obj;

        }

        public ProObject NewSetPro(ProObject TheProObj, params object[] ThePrameters)
        {

            ProObject Obj;

            myPrototype = TheProObj;

            if(myPrototype != null)
                Obj = new ProObject(TheProObj);
            else
                Obj = new ProObject();

            myConstructorAction(Obj, ThePrameters);

            return Obj;

        }

    }

}
