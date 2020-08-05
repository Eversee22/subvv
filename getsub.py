import base64
import json
import requests
from urllib import parse

DBG = False


def parsedata(data):
    fconf = open("confs", "w", encoding='utf-8')
    schemedata = {}
    datalen = len(data)
    print('length:', datalen)
    if datalen % 4 != 0:
        data += '=' * (4 - datalen % 4 )
    try:
        data2 = base64.urlsafe_b64decode(data)
        items = data2.split(b'\n')
    except Exception as e:
        print('Decoding Error', e)
        return schemedata
    cnt = 0
    for i in items:
        if len(i) > 0:
            cnt += 1
            p = i.find(b':')
            scheme = i[:p].decode().lower()
            t = i[p+3:]
            if scheme == "trojan":
                trlink = t.decode("utf-8")
                # print(scheme+"://"+trlink)
                if schemedata.get(scheme) is None:
                    schemedata[scheme] = [trlink]
                else:
                    schemedata[scheme].append(trlink)
                name = parse.unquote(trlink[trlink.find('#')+1:])
                fconf.write('tr '+name+' '+trlink+'\n')
                    
            elif scheme == "vmess":
                vmlink = base64.b64decode(t)
                try:
                    conf = json.loads(vmlink)
                    fconf.write('vm,'+conf['ps']+','+json.dumps(conf, ensure_ascii=False, separators=(',', ':'))+'\n')
                    if schemedata.get(scheme) is None:
                        schemedata[scheme] = [conf]
                    else:
                        schemedata[scheme].append(conf)
                    conff = '[%d]\nserver: %s\nport: %s\nid: %s\nalterId: %s\n' \
                         'net: %s\ntls: %s\npath: %s\nhost: %s\nps: %s\n' \
                         % (cnt, conf['add'], conf['port'], conf['id'], conf['aid'],
                            conf['net'], conf['tls'], conf['path'], conf['host'], conf['ps'])
                except Exception as e:
                    print("Json Read Error", e)
                    conff = ('[%d]\n' % cnt) + str(vmlink)
                if DBG:
                    print(conff)
            else:
                strt = t.decode("utf-8")
                print(scheme+"://"+strt)
                if schemedata.get(scheme) is None:
                    schemedata[scheme] = [strt]
                else:
                    schemedata[scheme].append(strt)                 
    fconf.close()
    return schemedata

def getsuburl(url, bkfile=None):
    print(url)
    try:
        r = requests.get(url)
        data = r.text
        if bkfile is not None:
            with open(bkfile, 'a') as f:
                f.write("%s,%s\n" % (url, data))
    except Exception as e:
        print('Url Error', e)
        return []
    return parsedata(data)


def getsubfile(filename):
    try:
        with open(filename) as f:
            data = f.read()
    except Exception as e:
        print('File Read Error', e)
        return []
    return parsedata(data)


def getsubraw(data):
    return parsedata(data)