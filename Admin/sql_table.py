import sys
from PyQt5.uic import loadUi
from PyQt5 import QtWidgets
from PyQt5.QtWidgets import QDialog, QApplication
import sqlite3

from PyQt5.uic.properties import QtCore


class MainWindow(QDialog):
    def __init__(self):
        super(MainWindow, self).__init__()
        loadUi("StatusImprumuturi.ui",self)
        self.tableWidget.setGeometry(80, 150,3500, 2500)
        self.tableWidget.setColumnWidth(0, 750)
        self.tableWidget.setColumnWidth(1, 750)
        self.tableWidget.setColumnWidth(2, 750)
        self.tableWidget.setColumnWidth(3, 750)
        self.tableWidget.setHorizontalHeaderLabels(["FirstName","LastName","Description","Date"])
        self.loaddata()
        self.pushButton.clicked.connect(self.loaddata)

    def loaddata(self):
        connection = sqlite3.connect('Portar.db')
        cur = connection.cursor()
        sqlstr = 'SELECT * FROM BorrowView ORDER BY Description'

        tablerow = 0
        results = cur.execute(sqlstr)
        self.tableWidget.setRowCount(100)
        for row in results:
            self.tableWidget.setItem(tablerow, 0, QtWidgets.QTableWidgetItem(row[0]))
            self.tableWidget.setItem(tablerow, 1, QtWidgets.QTableWidgetItem(row[1]))
            self.tableWidget.setItem(tablerow, 2, QtWidgets.QTableWidgetItem(row[2]))
            self.tableWidget.setItem(tablerow, 3, QtWidgets.QTableWidgetItem(row[3]))
            tablerow+=1



# main
app = QApplication(sys.argv)
mainwindow = MainWindow()
widget = QtWidgets.QStackedWidget()
widget.addWidget(mainwindow)
widget.showMaximized()
widget.show()

try:
    sys.exit(app.exec_())
except:
    print("Exiting")