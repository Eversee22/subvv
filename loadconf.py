import json
import sys
import os
import base64

rootd='directory where config files exist'
items=os.listdir(rootd)
print(len(items))
itemsdict={}
for i in items:
    key=i[:2]
    name=i[:i.find('.qv2ray')]
    if key in itemsdict.keys():
        itemsdict[key].append(name)
    else:
        itemsdict[key]=[name]
row,col=6,3
i=0
for k in itemsdict:
    print('--'+k+'--')
    vs=itemsdict[k]
    j=0
    for v in vs:
        j+=1
        if j%col==0:
            end='\n'
        else:
            end='  '
        print('[%d] '%i+v,end=end)
        i+=1
    print()
    
select=input('select(ie: 0,1,2,...;0-3): ')
slist=[]
if select.find('-') != -1:
    s,e=select.split('-')[0:2]
    for i in range(int(s),int(e),1):
        slist.append(i)
elif select.find(',') != -1:
    slist=[int(i) for i in select.split(',')]
else:
    sys.exit(1)
print(slist)

composels=[]
f=open('tmp','w')
pattern={"add":"","aid":0,"host":"","id":"","net":"","path":"","port":0,"ps":"","tls":"","v":2}
for i in slist:
    filename=items[i]
    jf=json.load(open(os.path.join(rootd,filename)))
    pattern['ps']=filename[:filename.find('.qv2ray')]
    pattern['add']=jf['outbounds'][0]['settings']['vnext'][0]['address']
    pattern['port']=jf['outbounds'][0]['settings']['vnext'][0]['port']
    pattern['aid']=jf['outbounds'][0]['settings']['vnext'][0]['users'][0]['alterId']
    pattern['id']=jf['outbounds'][0]['settings']['vnext'][0]['users'][0]['id']
    pattern['net']=jf['outbounds'][0]['streamSettings']['network']
    pattern['host']=jf['outbounds'][0]['streamSettings']['wsSettings']['headers']['Host']
    pattern['path']=jf['outbounds'][0]['streamSettings']['wsSettings']['path']
    pattern['tls']=jf['outbounds'][0]['streamSettings']['security']
    patterns=json.dumps(pattern,ensure_ascii=False,separators=(',',':'))  #dict to string
    composels.append(b'vmess://'+base64.b64encode(bytes(patterns,'utf-8')))
    f.write(patterns+'\n')
    
if len(composels) > 0:
    composition=b'\n'.join(composels)
    fncomp=base64.b64encode(composition).decode('ascii')
    f.write(fncomp)
print('over')
    
