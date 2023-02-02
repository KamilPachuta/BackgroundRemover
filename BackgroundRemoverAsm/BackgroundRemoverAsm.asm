.code
MyProc1 proc
;1 - rcx - *pixels [4 pixele], 2 - rdx - tablica 4 intów w której jest sklonowany kolor wybrany przez uzytkownika, 3 - r8, 4 - r9
; wpisz do r10 pixel1, wyciagnij r tego pixela i dodaj do r11

movdqu xmm0, [rcx]
movdqu xmm1, [rdx]



;pcmpeqb xmm0, xmm1 ; w xmm0 8xf oznacza zgodnosc ; 16 8-bitowych wartosci!
pcmpeqd xmm0, xmm1
;pcmppud xmm0, xmm1

movdqu xmm3, [rcx]



movdqa xmm2, xmm3 ; move xmm3 to xmm2
;pxor xmm15, xmm15
pcmpeqd xmm15, xmm15
pandn xmm0, xmm15
pand xmm2, xmm0 ; zero out values in xmm2 where xmm0 is 0xFFFFFFFF ; dokladnie przeciwienstwo

movdqu [rcx], xmm2




ret
MyProc1 endp
end