accountBalance = 1000
accountPassword = "085979"

def CheckBalance(withdrawal):
    if withdrawal > accountBalance:
        print("Amount exceeded balance!")
        CheckBalance(InputWithdrawalAmount())
    else:
        print(f"Withdrawal success: {withdrawal}")

def InputWithdrawalAmount():
    return  float(input("Please input amount that need to withdraw:"))

def AuthenticationCheck(inputPassword):
    i = 1
    while i <= 5:
        print(f"Authenticating {i}")
        i+=1
    if inputPassword == accountPassword:
        return True
    else:
        return False

inputPassword = str(input("Please input bank account password:"))
if AuthenticationCheck(inputPassword) is True:
    CheckBalance(InputWithdrawalAmount())
else:
    print("Access denied!")


