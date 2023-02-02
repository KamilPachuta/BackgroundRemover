.code
MyProc1 proc

movdqu xmm0, [rcx]		; Wpisz 4 pixele do xmm0
movdqu xmm1, [rdx]		; Wpisz 4x kolor do xmm1


pcmpeqd xmm0, xmm1		; Porównaj pixele xmm0 do xmm1

movdqu xmm2, [rcx]		; Wpisz 4 pixele do xmm2

pcmpeqd xmm15, xmm15	; Wype³nij xmm15 samymi 1

pandn xmm0, xmm15		; Operacja and-not na xmm0 i xmm15

pand xmm2, xmm0			; Operacja and na xmm0 i xmm2

movdqu [rcx], xmm2		; Wpisz przetoworzone pixele do tablicy


ret
MyProc1 endp
end