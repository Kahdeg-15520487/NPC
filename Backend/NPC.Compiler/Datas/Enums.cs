using System;
using System.Collections.Generic;
using System.Text;

namespace NPC.Compiler.Datas
{
    /// <summary>
    /// Condition data type.
    /// </summary>
    public enum ValType
    {
        /// <summary>
        /// GUID.
        /// </summary>
        Guid,

        /// <summary>
        /// Boolean.
        /// </summary>
        Bool,

        /// <summary>
        /// Integer.
        /// </summary>
        Int,

        /// <summary>
        /// String.
        /// </summary>
        String,

        /// <summary>
        /// DateTime.
        /// </summary>
        DateTime,

        Invalid = -1,
    }

    /// <summary>
    /// Operator type.
    /// </summary>
    public enum Operator
    {
        /// <summary>
        /// =
        /// </summary>
        Equal,

        /// <summary>
        /// Contain
        /// </summary>
        Contain,

        /// <summary>
        /// >
        /// </summary>
        Greater,

        /// <summary>
        /// <
        /// </summary>
        Less,

        /// <summary>
        /// >=
        /// </summary>
        GreaterOrEqual,

        /// <summary>
        /// <=
        /// </summary>
        LessOrEqual,

        /// <summary>
        /// In
        /// </summary>
        In,

        /// <summary>
        /// In
        /// </summary>
        IsEmpty,

        Invalid = -1,
    }

    public enum Conjunction
    {
        None,
        And,
        Or,
    }
}
