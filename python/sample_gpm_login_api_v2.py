import time
from GPMLoginAPI import GPMLoginAPI
# python3 -m pip install --upgrade pip
# python3 -m pip
# pip install selenium
from selenium.webdriver.chrome import service
from selenium.webdriver.chrome.options import Options

from UndetectChromeDriver import UndetectChromeDriver

api = GPMLoginAPI('http://127.0.0.1:62975') # Alert: copy url api on GPM Login App

#  Print list off profiles in GPMLogin -------------------------
print('PROFILES ----------------------------')
profiles = api.GetProfiles()
if(profiles != None):
    for profile in profiles:
        id = profile['id']
        name = profile['name']
        print(f"Id: {id} | Name: {name}")

print('CREATE PROFILE ------------------')
createdResult = api.Create("giaiphapmmo.net")
createdProfileId = None
if(createdResult != None):
    status = bool(createdResult['status'])
    if(status):
        createdProfileId = str(createdResult['profile_id'])
print(f"Created profile ID: {createdProfileId}")


print('START PROFILE ------------------')
startedResult = api.Start(createdProfileId);#, addinationArgs='--proxy-server="1.2.3.4:55"')
if(startedResult != None):
    status = bool(startedResult['status'])
    if(status):
        browserLocation = str(startedResult["browser_location"])
        seleniumRemoteDebugAddress = str(startedResult["selenium_remote_debug_address"])
        gpmDriverPath = str(startedResult["selenium_driver_location"])
        # Init selenium
        options = Options()
        options.debugger_address = seleniumRemoteDebugAddress
        myService  = service.Service(gpmDriverPath)
        driver = UndetectChromeDriver(service = myService, options=options)
        driver.GetByGpm("https://fingerprint.com/products/bot-detection/")
        time.sleep(10)
        driver.close()
        driver.quit()

print('DELETE PROFILE ------------------')
api.Delete(createdProfileId)
print(f"Deleted: {createdProfileId}")

print('ALL DONE, PRESS ENTER TO EXIT')
input() # pause