print("code:")
code = input()
print("number of colors:")
k = input()
print("number in convert:")
o = input()
print("number in neighbourhood:")
n = input()

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

print("rule " + str(rule))