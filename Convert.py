import math

print("code:")
code = int(input())
print("number of colors:")
k = int(input())
print("number in convert:")
o = int(input())
print("number in neighbourhood:")
n = int(input())

rule = 0

print("")

for place in range(0, k ** n):
    total = 0

    for i in range(0, n):
        high = place % (k ** (i + 1))
        low = place % (k ** i)
        final = (high - low) / (k ** i)
        total += final * (o ** i)

    high = code % (k ** (total + 1))
    low = code % (k ** total)
    final = (high - low) / (k ** total)
    rule += final * (k ** place)

print(rule)
srule = ""

for i in range(math.floor(math.log10(k ** (k ** n))) + 1, -1, -1):
    final = (rule % (10 ** (i + 1))) / (10 ** i)
    srule += str(math.floor(final))

print (srule)