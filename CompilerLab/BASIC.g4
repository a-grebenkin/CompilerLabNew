grammar BASIC;


program : 'if' condition 'then' expression;

condition : identifier '==' number;

expression
    : term
    | term '+' expression
    | term '-' expression
    ;

term
    : operand
    | operand '*' term
    | operand '/' term
    ;

operand
    : identifier
    | number
    | '(' expression ')'
    ;

identifier : LETTER LETTER*;

number : PDIGIT DIGIT*;

LETTER : [a-z];

PDIGIT : [1-9];

DIGIT : [0-9];

WS : [ \t\r\n]+ -> skip;