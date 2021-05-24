#pes_kraje <- read.csv(file = "https://share.uzis.cz/s/BRfppYFpNTddAy4/download?path=%2F&files=pes_kraje_verze2.csv", header = TRUE, sep = ";", dec = ".", encoding = "utf-8", fileEncoding = "UTF-8-BOM")
pes_cr <- read.csv(file = "https://share.uzis.cz/s/BRfppYFpNTddAy4/download?path=%2F&files=pes_CR_verze2.csv", header = TRUE, sep = ";", dec = ".", encoding = "utf-8", fileEncoding = "UTF-8-BOM")
dotplot(body ~ datum, data = pes_cr, aspect="fill" )
dotplot(incidence14_100 ~ datum, data = pes_cr, aspect = "fill")
boxplot(body ~ datum, data = pes_cr, aspect = "fill")