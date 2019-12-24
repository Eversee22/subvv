import base64
import sys,os
from urllib import request

def outget(out):
	try:
		allink=base64.b64decode(out).split(b'vmess://')
		allink=[l for l in allink if len(l)>0]
	except Exception:
		print('Error\n',e)
		sys.exit(1)
	links=[]
	print('get links')
	for l in allink:
		try:
			links.append(base64.b64decode(l))
		except Exception as e:
			pass
	print('write %d links'%len(links))
	with open('sublist','ab') as f:
		f.write(b'#\n')
		for l in links:
			f.write(l+b'\n')

def urlget(url):
	try:
		res=request.urlopen(url)
		out=res.read()
		res.close()
		print('get response')
		outget(out)
	except Exception as e:
		print('Error\n', e)
		sys.exit(1)

if __name__=='__main__':
	try:
		with open('inputurl') as f:
			url=f.readline().strip()
	except Exception as e:
		print('No file "inputurl"')
		sys.exit(1)
	urlget(url)
	print('over')
