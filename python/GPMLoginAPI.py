from tkinter.messagebox import NO
import requests
class GPMLoginAPI(object):
    API_START_PATH = "/v2/start"
    API_CREATE_PATH = "/v2/create"
    API_PROFILE_LIST_PATH = "/v2/profiles"
    API_DELETE_PATH = "/v2/delete"

    _apiUrl = ''
    def __init__(self, apiUrl: str):
        self._apiUrl = apiUrl

    def GetProfiles(self):
        try:
            url = f"{self._apiUrl}{self.API_PROFILE_LIST_PATH}"
            print(url)
            resp = requests.get(url)
            return resp.json()
        except:
            print('error GetProfiles()')
            return None

    def Create(self, name: str, proxy: str = '', isNoiseCanvas: bool = False):
        try:
            # Make api url
            url = f"{self._apiUrl}{self.API_CREATE_PATH}?name={name}&proxy={proxy}"
            if(isNoiseCanvas):
                url += "&canvas=on"
            # Call api
            resp = requests.get(url)
            return resp.json()
        except:
            return None

    def Start(self, profileId: str, remoteDebugPort: int = 0, addinationArgs: str = ''):
        try:
            # Make api url
            url = f"{self._apiUrl}{self.API_START_PATH}?profile_id={profileId}"
            if(remoteDebugPort > 0):
                url += f"&remote_debug_port={remoteDebugPort}"
            if(addinationArgs != ''):
                url += f"&addination_args={addinationArgs}"
            
            # call api
            resp = requests.get(url)
            return resp.json()
        except:
            return None
    
    def Delete(self, profileId: str, mode: int = 2):
        url = f"{self._apiUrl}{self.API_DELETE_PATH}?profile_id={profileId}&mode={mode}"
        requests.get(url)