# Using Arithmetic expressions
print ((10 + 2) * 100 / 5 - 200)

# Using functions in an expression
print(pow(2, 10))

# Using eval in an expression
print(eval( "2.5+2.5" ))

# same reference address for test0 and test1
test0 = "Learn Python"
print(id(test0))

# same reference address for test0 and test1
test1 = "Learn Python"
print(id(test1))

test2 = "Learn Python2"
print(id(test2))

# however, Python will also allocate the same memory address in the following two scenarios.
# The strings don’t have whitespaces and contain less than 20 characters.
# In case of Integers ranging between -5 to +255.
a = "learn python"
b = "learn python "
c = "learnpython"
d = "learn_python"

print(f'a address is {id(a)}')
print(f'b address is {id(b)}')
print(f'c address is {id(c)}')
print(f"d address is {id(d)}")

e = d
print(f"e address is {id(e)}")

my_tuple = (10, 20, 30)
my_tuple += (40, 50,)
print(f"my tupples are {my_tuple}")

object1 = "object1"
list_vowels = ['a','e','i']
list_vowels += ['o', 1, object1]
print(list_vowels)

