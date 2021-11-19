namespace NPC.Compiler.Datas
{
    public enum TokenType
    {
        // literal value token
        INTERGER,
        BOOL,
        STRING,
        GUID,
        DATETIME,
        IDENT,
        NULL,

        // operator keyword token
        AND,
        OR,
        NOT,

        ASSIGN,

        EQUAL,
        NOTEQUAL,

        LARGER,
        LARGEREQUAL,
        LESSER,
        LESSEREQUAL,

        CONTAINS,
        IN,
        ISEMPTY,

        // value type name keyword token
        TYPE,

        // language keyword token
        IF,
        MATCH,
        ELSE,
        ELIF,
        RETURN,
        TYPEOF,
        IS,

        // separator token
        LPAREN,
        RPAREN,
        LBRACE,
        RBRACE,
        LBRACKET,
        RBRACKET,

        COMMA,
        SEMICOLON,
        COLON,
        UNDERSCORE,

        EOF,
        ANY
    }

    public class Token
    {
        public TokenType type { get; private set; }
        public string lexeme { get; private set; }

        public Token(TokenType t, string l = null)
        {
            type = t;
            lexeme = l;
        }

        public override string ToString()
        {
            return string.Format("{0} : {1}", type, lexeme);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 91;
                hash = hash * 71 + lexeme.GetHashCode();
                hash = hash * 71 + type.GetHashCode();
                return hash;
            }
        }
    }
}
