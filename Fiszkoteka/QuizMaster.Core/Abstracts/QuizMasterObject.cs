using QuizMaster.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Core.Abstracts
{
    public abstract class QuizMasterObject
    {


        public override string ToString()
        {
            return this.ToPrettyString();
        }
    }
}
