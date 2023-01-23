.code
MyProc1 proc
;1 - rcx, 2 - rdx, 3 - r8, 4 - r9
; pixel - rcx
; color - rdx
; scope - r8
;ret rax


xor r12, r12				; Wyzerowanie r13
add r12, 16711680			; Wpisz maske na kolor R

mov r10, rdx				; Wpisanie koloru do r10
and r10, r12				; Zapisz kolor R z koloru w r10
shr r10, 16					; Przesuniecie bitowe o 16

mov r11, rcx				; Wpisanie pixela do r11
and r11, r12				; Zapisz kolor R z pixela w r11
shr r11, 16					; Przesuniecie bitowe o 16

mov r12, r11				; Wpisz kolor R z pixela do r12
sub r12, r8					; pixel.r - scope w r12
add r11, r8					; pixel.r + scope w r11
cmp r12, r10
jg koniec
cmp r11, r10
jl koniec



xor r12, r12				; Wyzerowanie r13
add r12, 65280				; Wpisz maske na kolor G

mov r10, rdx				; Wpisanie koloru do r10
and r10, r12				; Zapisz kolor G z koloru w r10
shr r10, 16					; Przesuniecie bitowe o 8

mov r11, rcx				; Wpisanie pixela do r11
and r11, r12				; Zapisz kolor G z pixela w r11
shr r11, 16					; Przesuniecie bitowe o 8

mov r12, r11				; Wpisz kolor G z pixela do r12
sub r12, r8					; pixel.g - scope w r12
add r11, r8					; pixel.g + scope w r11
cmp r12, r10
jg koniec
cmp r11, r10
jl koniec


xor r12, r12				; Wyzerowanie r13
add r12, 255				; Wpisz maske na kolor B

mov r10, rdx				; Wpisanie koloru do r10
and r10, r12				; Zapisz kolor B z koloru w r10
shr r10, 16					; Przesuniecie bitowe o 8

mov r11, rcx				; Wpisanie pixela do r11
and r11, r12				; Zapisz kolor B z pixela w r11
shr r11, 16					; Przesuniecie bitowe o 8

mov r12, r11				; Wpisz kolor B z pixela do r12
sub r12, r8					; pixel.b - scope w r12
add r11, r8					; pixel.b + scope w r11
cmp r12, r10
jg koniec
cmp r11, r10
jl koniec

xor rcx, rcx


koniec:

mov rax, rcx ; Zwrocenie int przez rax 

ret
MyProc1 endp
end
