# bunch of functions used to do the repetitive tasks

def iter_matrix_members(row: int, column: int, func) -> str:
    output = ''
    for c in range(column):
        for r in range(row):
            output += func(c + 1, r + 1)
    
    return output

def iter_matrix_multiplication(size: int, func) -> str:
    output = ''
    for column in range(size):
        for row in range(size):
            for i in range(size):
                lhs = column + 1, i + 1
                rhs = i + 1, row + 1
                output += func(lhs, rhs)
    
    return output

vec_mems = ['X', 'Y', 'Z', 'W']

def iter_vector_members(size: int, func) -> str:
    output = ''
    for i in range(size):
        output += func(i + 1, vec_mems[i])
    
    return output

def m3x3_op_float(operator: str) -> str:
    return iter_matrix_members(3, 3, lambda c, r: 'a.M{0}{1}{2}b{3}'.format(c, r, operator, ',\n' if r == 3 else ','))
def float_op_m3x3(operator: str) -> str:
    return iter_matrix_members(3, 3, lambda c, r: 'a{2}b.M{0}{1}{3}'.format(c, r, operator, ',\n' if r == 3 else ','))
def m3x3_mul_m3x3(operator: str) -> str:
    return iter_matrix_multiplication(3, lambda lhs, rhs: 'a.M{0}{1}{5}b.M{2}{3}{4}'.format(lhs[0], lhs[1], rhs[0], rhs[1], '+' if lhs[1] != 3 else ',\n', operator))
def m4x4_mul_m4x4(operator: str) -> str:
    return iter_matrix_multiplication(4, lambda lhs, rhs: 'a.M{0}{1}{4}b.M{2}{3}{5}'.format(lhs[0], lhs[1], rhs[0], rhs[1], operator, '+' if lhs[1] != 4 else ',\n'))
def m4x4_op_float(operator: str) -> str:
    return iter_matrix_members(4, 4, lambda c, r: 'a.M{0}{1}{2}b{3}'.format(c, r, operator, ',\n' if r == 4 else ','))
def m3x3mems(func) -> str:
    return iter_matrix_members(3, 3, func)
def m4x4mems(func) -> str:
    return iter_matrix_members(4, 4, func)
def m3x3mul(func) -> str:
    return iter_matrix_multiplication(3, func)
def m4x4mul(func) -> str:
    return iter_matrix_multiplication(4, func)
def vec2mems(func) -> str:
    return iter_vector_members(2, func)
def vec3mems(func) -> str:
    return iter_vector_members(3, func)
def vec4mems(func) -> str:
    return iter_vector_members(4, func)

# print(m3x3mems(lambda c, r: 'a.M{0}{1}-b.M{0}{1},'.format(c, r)))
# print(vec4mems(lambda i, n: 'builder.Append({0}).ToString(format, formatProvider);builder.Append(seperator);\n'.format(n)))