#!/bin/bash

echo "Opciones:"
echo " "
echo "run -> Ejecutar el proyecto"
echo "report -> Compilar y generar informe del proyecto"
echo "slides -> Compilar y generar presentación del proyecto"
echo "show_report -> Ejecuta un programa para visualizar el informe del proyecto"
echo "show_slides -> Ejecuta un programa para visualizar la presentación del proyecto"
echo "clean -> Limpiar Ficheros innecesarios"

echo " "

echo -n "input: "
read input

function run {

cd ".."
make dev

}

function report {

cd ".."
cd "Informe"
pdflatex "Informe.tex"

}

function slides {

cd ".."
cd "Presentación"
pdflatex "Presentación.tex"

}

function show_report {

cd ".."
cd "Informe"
if [ ! -f "Informe.pdf" ]; then
	report
else 
	echo "Ya existe el pdf"
fi

echo "Seleccione su visualizador o pulse enter para usar uno por defecto"
read v

if [ ! $v == "" ]; then

echo "Se abrirá el pdf con $v"
$v "Informe.pdf"

elif [[ "$OSTYPE" == "linux-gnu"* ]]; then
	xdg-open "Informe.pdf"
elif [[ "$OSTYPE" == "darwin"* ]]; then
	open "Informe.pdf"
else
	start "Informe.pdf"
fi

}

function show_slides {
cd ".."
cd "Presentación"
if [ ! -f "Presentación.pdf" ]; then
	slides
else 
	echo "Ya existe el pdf"
fi

echo "Seleccione su visualizador o pulse enter para usar uno por defecto"
read v

if [ ! $v == "" ]; then

echo "Se abrirá el pdf con $v"
$v "Presentación.pdf"

elif [[ "$OSTYPE" == "linux-gnu"* ]]; then
	xdg-open "Presentación.pdf"
elif [[ "$OSTYPE" == "darwin"* ]]; then
	open "Presentación.pdf"
else
	start "Presentación.pdf"
fi

}

function clean {

cd ".."
cd  "Informe"
    rm -f *.log *.aux *.out *.fdb_latexmk *.fls *.synctex.gz
cd ".."
cd "Presentación"
    rm -f *.log *.aux *.out *.fdb_latexmk *.fls *.synctex.gz
cd ".."
    find . -type d -name bin -exec rm -rf {} +
    find . -type d -name obj -exec rm -rf {} +
    find . -type d -name .vs -exec rm -rf {} +

}
case "$input" in
run)
	run
;;
report)
	report
;;
slides)
	slides
;;
show_report)
	show_report
;;
show_slides)
	show_slides
;;
clean)
	clean
;;
*)
	echo "Opción inválida"
esac
