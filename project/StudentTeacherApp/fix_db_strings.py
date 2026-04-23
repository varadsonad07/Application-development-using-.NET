from pathlib import Path
root = Path('Pages')
for p in root.rglob('*.cs'):
    text = p.read_text(encoding='utf-8')
    new = text
    new = new.replace('new MySqlConnection("Server=localhost;Port=3306;Database=dktestudent;Uid=root;Pwd=admin123;")','Database.GetConnection()')
    new = new.replace('new MySqlConnection("Server=localhost;Port=3306;Database=dktestudent;Uid=root;Pwd=admin123;AllowPublicKeyRetrieval=True;SslMode=Preferred;")','Database.GetConnection()')
    if 'using MySql.Data.MySqlClient;' in new and 'using StudentTeacherApp.Data;' not in new:
        new = new.replace('using MySql.Data.MySqlClient;\n','using MySql.Data.MySqlClient;\nusing StudentTeacherApp.Data;\n')
    if new != text:
        p.write_text(new, encoding='utf-8')
        print(f'Updated {p}')
