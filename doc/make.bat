doxygen Doxyfile >stdout.txt 2>stderror.txt
cd latex
call make.bat
copy refman.pdf ..\..\RefMan.pdf
