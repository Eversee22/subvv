import base64
import os,sys
import json

pre='dm1lc3M6'
try:
	with open('inputb64') as f:
		out=f.readline().strip()
except Exception as e:
	print('No file "inputb64"')
	sys.exit(1)
print('get b64',len(out))
if out.find(pre)==-1:
	print('Unsupport encoding')
	sys.exit(1)	
vms=out.split(pre)
vms=[v for v in vms if len(v)>0]
allink=[]
for v in vms:
	try:
		allink.append(base64.b64decode(v))
	except:
		pass
cnt=0
f=None
if len(sys.argv)>1:
	f=open(sys.argv[1],'w')	
for i,l in enumerate(allink):
	try:
		l=l[2:]
		lks=l.split(b'vmess://')
		for lk in lks:
			conf=base64.b64decode(lk)
			cnt+=1
			# print('[%d]'%(cnt))
			conf=json.loads(conf)
			strr='[%d]\nhost: %s\nport: %s\nid: %s\nalterId: %s\nnet: %s\ntls: %s\npath: %s\nadd: %s\nps: %s\n'%(
				cnt,conf['host'],conf['port'],conf['id'],conf['aid'],conf['net'],conf['tls'],conf['path'],conf['add'],conf['ps'])
			print(strr)
			if f is not None:
				f.write(strr+'\n')
	except:
		pass
if f is not None:
	f.close()
	